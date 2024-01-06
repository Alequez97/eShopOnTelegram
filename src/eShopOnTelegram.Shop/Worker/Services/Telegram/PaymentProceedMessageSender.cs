using System.Text;

using eShopOnTelegram.Domain.Dto.Orders;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Keys;
using eShopOnTelegram.Shop.Worker.Services.Mappers;
using eShopOnTelegram.Shop.Worker.Services.Payment.Interfaces;
using eShopOnTelegram.Utils.Configuration;

using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.Shop.Worker.Services.Telegram;

public class PaymentProceedMessageSender
{
	private readonly ITelegramBotClient _telegramBot;
	private readonly IOrderService _orderService;
	private readonly IEnumerable<IPaymentTelegramButtonProvider> _paymentTelegramButtonGenerators;
	private readonly CurrencyCodeToSymbolMapper _currencyCodeToSymbolMapper;
	private readonly IApplicationContentStore _applicationContentStore;
	private readonly PaymentSettings _paymentSettings;

	public PaymentProceedMessageSender(
		ITelegramBotClient telegramBot,
		IOrderService orderService,
		IEnumerable<IPaymentTelegramButtonProvider> paymentTelegramButtonGenerators,
		CurrencyCodeToSymbolMapper currencyCodeToSymbolMapper,
		IApplicationContentStore applicationContentStore,
		AppSettings appSettings)
	{
		_telegramBot = telegramBot;
		_orderService = orderService;
		_paymentTelegramButtonGenerators = paymentTelegramButtonGenerators;
		_currencyCodeToSymbolMapper = currencyCodeToSymbolMapper;
		_applicationContentStore = applicationContentStore;
		_paymentSettings = appSettings.PaymentSettings;
	}

	public async Task SendProceedToPaymentAsync(long chatId, OrderDto order, CancellationToken cancellationToken)
	{
		if (_paymentSettings.AllPaymentsDisabled)
		{
			await _telegramBot.SendTextMessageAsync(chatId, await _applicationContentStore.GetValueAsync(ApplicationContentKey.Payment.NoEnabledPayments, CancellationToken.None));
			return;
		}

		var paymentMethodButtons = _paymentTelegramButtonGenerators
			.Where(buttonGenerator => buttonGenerator.PaymentMethodEnabled(_paymentSettings))
			.Select(buttonGenerator => new List<InlineKeyboardButton>() { buttonGenerator.GetInvoiceGenerationButton() });

		InlineKeyboardMarkup inlineKeyboard = new(paymentMethodButtons);

		var message = new StringBuilder();
		var currencySymbol = _currencyCodeToSymbolMapper.GetCurrencySymbol(_paymentSettings.MainCurrency);

		message
			.AppendLine($"{await _applicationContentStore.GetValueAsync(ApplicationContentKey.Order.OrderSummaryTitle, CancellationToken.None)}")
			.AppendLine(new string('~', 20));

		foreach (var orderCartItem in order.CartItems)
		{
			message
			.AppendLine(orderCartItem.GetFormattedMessage(currencySymbol));
		};

		message
			.AppendLine()
			.AppendLine($"{await _applicationContentStore.GetValueAsync(ApplicationContentKey.Order.TotalPriceTitle, CancellationToken.None)}: {order.TotalPrice}{currencySymbol}")
			.AppendLine(new string('~', 20));

		message
			.AppendLine(await _applicationContentStore.GetValueAsync(ApplicationContentKey.Payment.ChoosePaymentMethod, CancellationToken.None));

		await _telegramBot.SendTextMessageAsync(
			chatId: chatId,
			text: message.ToString(),
			parseMode: ParseMode.Html,
			replyMarkup: inlineKeyboard,
			cancellationToken: cancellationToken);
	}
}
