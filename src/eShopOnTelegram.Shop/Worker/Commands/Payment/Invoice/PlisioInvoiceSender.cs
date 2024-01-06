using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.ExternalServices.Services.Plisio;
using eShopOnTelegram.Persistence.Entities.Orders;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Keys;
using eShopOnTelegram.Shop.Worker.Commands.Interfaces;
using eShopOnTelegram.Shop.Worker.Constants;
using eShopOnTelegram.Shop.Worker.Extensions;
using eShopOnTelegram.Utils.Configuration;

using Refit;

using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.Shop.Worker.Commands.Payment.Invoice;

public class PlisioInvoiceSender : ITelegramCommand
{
	private readonly ITelegramBotClient _telegramBot;
	private readonly IPlisioClient _plisioClient;
	private readonly IOrderService _orderService;
	private readonly IPaymentService _paymentService;
	private readonly PaymentSettings _paymentSettings;
	private readonly IApplicationContentStore _applicationContentStore;
	private readonly ILogger<PlisioInvoiceSender> _logger;

	public PlisioInvoiceSender(
		ITelegramBotClient telegramBot,
		IPlisioClient plisioClient,
		IOrderService orderService,
		IPaymentService paymentService,
		AppSettings appSettings,
		IApplicationContentStore applicationContentStore,
		ILogger<PlisioInvoiceSender> logger)
	{
		_telegramBot = telegramBot;
		_plisioClient = plisioClient;
		_orderService = orderService;
		_paymentService = paymentService;
		_paymentSettings = appSettings.PaymentSettings;
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
				await _telegramBot.SendTextMessageAsync(chatId, await _applicationContentStore.GetValueAsync(ApplicationContentKey.Order.InvoiceGenerationFailedErrorMessage, CancellationToken.None));
				return;
			}
			if (getOrdersResponse.Data.PaymentMethodSelected)
			{
				await _telegramBot.SendTextMessageAsync(chatId, await _applicationContentStore.GetValueAsync(ApplicationContentKey.Order.PaymentMethodAlreadySelected, CancellationToken.None));
				return;
			}

			var activeOrder = getOrdersResponse.Data;

			var createPlisioInvoiceResponse = await _plisioClient.CreateInvoiceAsync(
				_paymentSettings.Plisio.ApiToken,
				_paymentSettings.MainCurrency,
				(int)Math.Ceiling(activeOrder.TotalPrice),
				activeOrder.OrderNumber,
				_paymentSettings.Plisio.CryptoCurrency);

			var response = await _paymentService.UpdateOrderPaymentMethod(activeOrder.OrderNumber, PaymentMethod.Plisio);
			if (response.Status != ResponseStatus.Success)
			{
				throw new Exception("Failed to update order payment method in Plisio Invoice Sender TG Command.");
			}

			InlineKeyboardMarkup inlineKeyboard = new(new[]
			{
                // first row
                new []
				{
					InlineKeyboardButton.WithUrl(await _applicationContentStore.GetValueAsync(ApplicationContentKey.Payment.ProceedToPayment, CancellationToken.None), createPlisioInvoiceResponse.Data.InvoiceUrl),
				},
			});

			await _telegramBot.SendTextMessageAsync(
				chatId: chatId,
				text: await _applicationContentStore.GetValueAsync(ApplicationContentKey.Payment.InvoiceReceiveMessage, CancellationToken.None),
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
