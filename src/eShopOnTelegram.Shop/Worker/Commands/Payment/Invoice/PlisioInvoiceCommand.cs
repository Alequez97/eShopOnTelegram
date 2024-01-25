using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.ExternalServices.Services.Plisio;
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

public class PlisioInvoiceCommand : ITelegramCommand
{
	private readonly ITelegramBotClient _telegramBot;
	private readonly IPlisioClient _plisioClient;
	private readonly IOrderService _orderService;
	private readonly IPaymentService _paymentService;
	private readonly AppSettings _appSettings;
	private readonly ITranslationsService _translationsService;
	private readonly IApplicationContentStore _applicationContentStore;
	private readonly ILogger<PlisioInvoiceCommand> _logger;

	public PlisioInvoiceCommand(
		ITelegramBotClient telegramBot,
		IPlisioClient plisioClient,
		IOrderService orderService,
		IPaymentService paymentService,
		AppSettings appSettings,
		ITranslationsService translationsService,
		IApplicationContentStore applicationContentStore,
		ILogger<PlisioInvoiceCommand> logger)
	{
		_telegramBot = telegramBot;
		_plisioClient = plisioClient;
		_orderService = orderService;
		_paymentService = paymentService;
		_translationsService = translationsService;
		_appSettings = appSettings;
		_applicationContentStore = applicationContentStore;
		_logger = logger;
	}

	public async Task SendResponseAsync(Update update)
	{
		var chatId = update.CallbackQuery.From.Id;

		try
		{
			var getOrdersResponse = await _orderService.GetUnpaidOrderByTelegramIdAsync(chatId, CancellationToken.None);

			if (getOrdersResponse.Status != ResponseStatus.Success)
			{
				await _telegramBot.SendTextMessageAsync(chatId, await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.InvoiceGenerationFailedErrorMessage, CancellationToken.None));
				return;
			}
			if (getOrdersResponse.Data.PaymentMethodSelected)
			{
				await _telegramBot.SendTextMessageAsync(chatId, await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.PaymentMethodAlreadySelected, CancellationToken.None));
				return;
			}

			var activeOrder = getOrdersResponse.Data;

			var createPlisioInvoiceResponse = await _plisioClient.CreateInvoiceAsync(
				_appSettings.PaymentSettings.Plisio.ApiToken,
				_appSettings.PaymentSettings.MainCurrency,
				(int)Math.Ceiling(activeOrder.TotalPrice),
				activeOrder.OrderNumber,
				_appSettings.PaymentSettings.Plisio.CryptoCurrency);

			var response = await _paymentService.UpdateOrderPaymentMethod(activeOrder.OrderNumber, PaymentMethod.Plisio);
			if (response.Status != ResponseStatus.Success)
			{
				throw new Exception($"[{nameof(PlisioInvoiceCommand)}]: Failed to update order payment method.");
			}

			var buttonText = await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.ProceedToPayment, CancellationToken.None);
			InlineKeyboardMarkup inlineKeyboard = new(new[]
			{
                // first row
                new []
				{
					InlineKeyboardButton.WithUrl(buttonText, createPlisioInvoiceResponse.Data.InvoiceUrl),
				},
			});

			await _telegramBot.SendTextMessageAsync(
				chatId: chatId,
				text: await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.InvoiceReceived, CancellationToken.None),
				replyMarkup: inlineKeyboard,
				cancellationToken: CancellationToken.None);
		}
		catch (ApiException apiException)
		{
			_logger.LogError(apiException, $"{apiException.Message}\n{apiException.Content}");
			await _telegramBot.SendDefaultErrorMessageAsync(chatId, _applicationContentStore, _logger, CancellationToken.None);
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, exception.Message);
			await _telegramBot.SendDefaultErrorMessageAsync(chatId, _applicationContentStore, _logger, CancellationToken.None);
		}
	}

	public Task<bool> IsResponsibleForUpdateAsync(Update update)
	{
		return Task.FromResult(update.Type == UpdateType.CallbackQuery && update.CallbackQuery.Data.Equals(PaymentMethodConstants.Plisio));
	}
}
