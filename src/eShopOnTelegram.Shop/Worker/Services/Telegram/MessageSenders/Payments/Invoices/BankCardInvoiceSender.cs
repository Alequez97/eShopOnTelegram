using eShopOnTelegram.Domain.Dto.Orders;
using eShopOnTelegram.Persistence.Entities.Payments;
using eShopOnTelegram.Shop.Worker.Extensions;
using eShopOnTelegram.Shop.Worker.Services.Telegram.MessageSenders.Payments.Invoices.Interfaces;
using eShopOnTelegram.Translations.Constants;
using eShopOnTelegram.Translations.Interfaces;
using eShopOnTelegram.Utils.Configuration;

namespace eShopOnTelegram.Shop.Worker.Services.Telegram.MessageSenders.Payments.Invoices;
public class BankCardInvoiceSender : IInvoiceSender
{
	private readonly ITelegramBotClient _telegramBot;
	private readonly IProductAttributeService _productAttributeService;
	private readonly ITranslationsService _translationsService;
	private readonly AppSettings _appSettings;

	public BankCardInvoiceSender(
		ITelegramBotClient telegramBot,
		IProductAttributeService productAttributeService,
		ITranslationsService translationsService,
		AppSettings appSettings)
	{
		_telegramBot = telegramBot;
		_productAttributeService = productAttributeService;
		_translationsService = translationsService;
		_appSettings = appSettings;
	}

	public async Task SendInvoiceAsync(long telegramId, OrderDto orderDto, CancellationToken cancellationToken)
	{
		await _telegramBot.SendInvoiceAsync(
			telegramId,
			await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.OrderNumber, CancellationToken.None) + " " + orderDto.OrderNumber,
			" ", // TODO: Add list of purchasing products
			orderDto.OrderNumber,
			_appSettings.PaymentSettings.Card.ApiToken,
			_appSettings.PaymentSettings.MainCurrency,
			await orderDto.CartItems.GetPaymentLabeledPricesAsync(_productAttributeService, CancellationToken.None),
			needShippingAddress: false,
			needPhoneNumber: true,
			needName: true,
			cancellationToken: CancellationToken.None
		);
	}

	public PaymentMethod PaymentMethod => PaymentMethod.Card;
}
