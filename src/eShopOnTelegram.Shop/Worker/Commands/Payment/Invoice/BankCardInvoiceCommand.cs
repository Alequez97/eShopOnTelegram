using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Persistence.Entities.Orders;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.Shop.Worker.Commands.Interfaces;
using eShopOnTelegram.Shop.Worker.Constants;
using eShopOnTelegram.Shop.Worker.Extensions;
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

			if (getOrdersResponse.Data.PaymentMethodSelected)
			{
				await _telegramBot.SendTextMessageAsync(chatId, await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.PaymentMethodAlreadySelected, CancellationToken.None));
				return;
			}

			var activeOrder = getOrdersResponse.Data;

			await _telegramBot.SendInvoiceAsync(
				chatId,
				await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.OrderNumber, CancellationToken.None) + " " + getOrdersResponse.Data.OrderNumber,
				" ", // TODO: Add list of purchasing products
				activeOrder.OrderNumber,
				_appSettings.PaymentSettings.Card.ApiToken,
				_appSettings.PaymentSettings.MainCurrency,
				await activeOrder.CartItems.GetPaymentLabeledPricesAsync(_productAttributeService, CancellationToken.None),
				needShippingAddress: true,
				needPhoneNumber: true,
				needName: true,
				cancellationToken: CancellationToken.None
			);

			var response = await _paymentService.UpdateOrderPaymentMethod(activeOrder.OrderNumber, PaymentMethod.Card);
			if (response.Status != ResponseStatus.Success)
			{
				throw new Exception("Failed to update order payment method in BankCardInvoiceCommand telegram command.");
			}
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
