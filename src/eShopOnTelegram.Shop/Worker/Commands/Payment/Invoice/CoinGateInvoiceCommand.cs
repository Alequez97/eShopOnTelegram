using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.ExternalServices.Services.CoinGate;
using eShopOnTelegram.ExternalServices.Services.CoinGate.Requests;
using eShopOnTelegram.Persistence.Entities.Orders;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.Shop.Worker.Commands.Interfaces;
using eShopOnTelegram.Shop.Worker.Constants;
using eShopOnTelegram.Shop.Worker.Extensions;
using eShopOnTelegram.Translations.Constants;
using eShopOnTelegram.Translations.Interfaces;
using eShopOnTelegram.Utils.Configuration;

using Refit;

using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.Shop.Worker.Commands.Payment.Invoice;

public class CoinGateInvoiceCommand : ITelegramCommand
{
	private readonly ITelegramBotClient _telegramBot;
	private readonly ICoinGateClient _coinGateClient;
	private readonly IOrderService _orderService;
	private readonly IPaymentService _paymentService;
	private readonly AppSettings _appSettings;
	private readonly ITranslationsService _translationsService;
	private readonly IApplicationContentStore _applicationContentStore;
	private readonly ILogger<CoinGateInvoiceCommand> _logger;

	public CoinGateInvoiceCommand(
		ITelegramBotClient telegramBot,
		ICoinGateClient coinGateClient,
		IOrderService orderService,
		IPaymentService paymentService,
		AppSettings appSettings,
		ITranslationsService translationsService,
		IApplicationContentStore applicationContentStore,
		ILogger<CoinGateInvoiceCommand> logger)
	{
		_telegramBot = telegramBot;
		_coinGateClient = coinGateClient;
		_orderService = orderService;
		_paymentService = paymentService;
		_translationsService = translationsService;
		_appSettings = appSettings;
		_applicationContentStore = applicationContentStore;
		_logger = logger;
	}

	public async Task SendResponseAsync(Update update)
	{
		var telegramId = update.CallbackQuery.From.Id;

		try
		{
			var getOrdersResponse = await _orderService.GetUnpaidOrderByTelegramIdAsync(telegramId, CancellationToken.None);

			if (getOrdersResponse.Status != ResponseStatus.Success)
			{
				await _telegramBot.SendTextMessageAsync(telegramId, await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.InvoiceGenerationFailedErrorMessage, CancellationToken.None));
				return;
			}
			if (getOrdersResponse.Data.PaymentMethodSelected)
			{
				await _telegramBot.SendTextMessageAsync(telegramId, await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.PaymentMethodAlreadySelected, CancellationToken.None));
				return;
			}

			var activeOrder = getOrdersResponse.Data;


			var webhookValidationToken = Guid.NewGuid().ToString();

			var updateValidationTokenResponse = await _paymentService.UpdateValidationTokenAsync(webhookValidationToken, activeOrder.OrderNumber, CancellationToken.None);
			if (updateValidationTokenResponse.Status != ResponseStatus.Success)
			{
				await _telegramBot.SendTextMessageAsync(telegramId, await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.InvoiceGenerationFailedErrorMessage, CancellationToken.None));
				return;
			}

			var createCoinGateInvoiceRequest = new CreateCoinGateInvoiceRequest()
			{
				PriceAmount = (int)Math.Ceiling(activeOrder.TotalPrice),
				PriceCurrency = _appSettings.PaymentSettings.MainCurrency,
				ReceiveCurrency = "DO_NOT_CONVERT",
				OrderNumber = activeOrder.OrderNumber,
				CallbackUrl = $"{_appSettings.TelegramBotSettings.ShopAppUrl}/api/webhook/coinGate",
				CustomValidationToken = webhookValidationToken,
			};

			var createCoinGateInvoiceResponse = await _coinGateClient.CreateInvoiceAsync(
				$"Bearer {_appSettings.PaymentSettings.CoinGate.ApiToken}",
				createCoinGateInvoiceRequest
			);

			var response = await _paymentService.UpdateOrderPaymentMethodAsync(activeOrder.OrderNumber, PaymentMethod.CoinGate, CancellationToken.None);
			if (response.Status != ResponseStatus.Success)
			{
				throw new Exception($"[{nameof(CoinGateInvoiceCommand)}]: Failed to update order payment method.");
			}

			var buttonText = await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.ProceedToPayment, CancellationToken.None);
			InlineKeyboardMarkup inlineKeyboard = new(new[]
			{
                // first row
                new []
				{
					InlineKeyboardButton.WithUrl(buttonText, createCoinGateInvoiceResponse.PaymentUrl),
				},
			});

			await _telegramBot.SendTextMessageAsync(
				chatId: telegramId,
				text: await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.InvoiceReceived, CancellationToken.None),
				replyMarkup: inlineKeyboard,
				cancellationToken: CancellationToken.None);
		}
		catch (ApiException apiException)
		{
			_logger.LogError(apiException, $"{apiException.Message}\n{apiException.Content}");
			await _telegramBot.SendDefaultErrorMessageAsync(telegramId, _applicationContentStore, _logger, CancellationToken.None);
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, exception.Message);
			await _telegramBot.SendDefaultErrorMessageAsync(telegramId, _applicationContentStore, _logger, CancellationToken.None);
		}
	}

	public Task<bool> IsResponsibleForUpdateAsync(Update update)
	{
		return Task.FromResult(update.Type == UpdateType.CallbackQuery && update.CallbackQuery.Data.Equals(PaymentMethodConstants.CoinGate));
	}
}
