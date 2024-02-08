using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Persistence.Entities.Payments;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.Shop.Worker.Commands.Interfaces;
using eShopOnTelegram.Shop.Worker.Constants;
using eShopOnTelegram.Shop.Worker.Extensions;
using eShopOnTelegram.Shop.Worker.Services.Telegram.MessageSenders.Payments.Invoices;
using eShopOnTelegram.Translations.Constants;
using eShopOnTelegram.Translations.Interfaces;
using eShopOnTelegram.Utils.Configuration;

namespace eShopOnTelegram.Shop.Worker.Commands.Payment.Invoice;

public class BankCardInvoiceCommand : ITelegramCommand
{
	private readonly ITelegramBotClient _telegramBot;
	private readonly IProductAttributeService _productAttributeService;
	private readonly IOrderService _orderService;
	private readonly IPaymentService _paymentService;
	private readonly AppSettings _appSettings;
	private readonly IApplicationContentStore _applicationContentStore;
	private readonly ITranslationsService _translationsService;
	private readonly ILogger<BankCardInvoiceCommand> _logger;

	public BankCardInvoiceCommand(
		ITelegramBotClient telegramBot,
		IProductAttributeService productAttributeService,
		IOrderService orderService,
		IPaymentService paymentService,
		AppSettings appSettings,
		IApplicationContentStore applicationContentStore,
		ITranslationsService translationsService,
		ILogger<BankCardInvoiceCommand> logger)
	{
		_telegramBot = telegramBot;
		_productAttributeService = productAttributeService;
		_orderService = orderService;
		_paymentService = paymentService;
		_appSettings = appSettings;
		_applicationContentStore = applicationContentStore;
		_translationsService = translationsService;
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

			var activeOrder = getOrdersResponse.Data;
			
			if (activeOrder.PaymentMethodSelected)
			{
				await _telegramBot.SendTextMessageAsync(chatId, await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.PaymentMethodAlreadySelected, CancellationToken.None));
				return;
			}

			var updatePaymentMethodResponse = await _paymentService.UpdateOrderPaymentMethodAsync(activeOrder.OrderNumber, PaymentMethod.Card, CancellationToken.None);
			
			if (updatePaymentMethodResponse.Status != ResponseStatus.Success)
			{
				_logger.LogError("[BankCardInvoiceCommand]: Failed to update payment method for order number {orderNumber}", activeOrder.OrderNumber);
				await _telegramBot.SendTextMessageAsync(chatId, await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.InvoiceGenerationFailedErrorMessage, CancellationToken.None));
				return;
			}

			var bankCardInvoiceSender = new BankCardInvoiceSender(_telegramBot, _productAttributeService, _translationsService, _appSettings);
			await bankCardInvoiceSender.SendInvoiceAsync(chatId, activeOrder, CancellationToken.None);
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, exception.Message);
			await _telegramBot.SendDefaultErrorMessageAsync(chatId, _applicationContentStore, _logger, CancellationToken.None);
		}
	}

	public Task<bool> IsResponsibleForUpdateAsync(Update update)
	{
		return Task.FromResult(update.Type == UpdateType.CallbackQuery && update.CallbackQuery.Data.Equals(PaymentMethodConstants.BankCard));
	}
}
