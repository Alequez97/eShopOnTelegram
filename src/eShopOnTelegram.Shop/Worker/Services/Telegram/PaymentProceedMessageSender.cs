using System.Text;

using eShopOnTelegram.Domain.Dto.Orders;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Keys;
using eShopOnTelegram.Shop.Worker.Services.Mappers;
using eShopOnTelegram.Shop.Worker.Services.Payment.Interfaces;
using eShopOnTelegram.Translations.Constants;
using eShopOnTelegram.Translations.Interfaces;
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
	private readonly ITranslationsService _translationsService;
	private readonly AppSettings _appSettings;

	public PaymentProceedMessageSender(
		ITelegramBotClient telegramBot,
		IOrderService orderService,
		IEnumerable<IPaymentTelegramButtonProvider> paymentTelegramButtonGenerators,
		CurrencyCodeToSymbolMapper currencyCodeToSymbolMapper,
		IApplicationContentStore applicationContentStore,
		ITranslationsService translationsService,
		AppSettings appSettings)
	{
		_telegramBot = telegramBot;
		_orderService = orderService;
		_paymentTelegramButtonGenerators = paymentTelegramButtonGenerators;
		_currencyCodeToSymbolMapper = currencyCodeToSymbolMapper;
		_applicationContentStore = applicationContentStore;
		_translationsService = translationsService;
		_appSettings = appSettings;
	}

	public async Task SendProceedToPaymentAsync(long chatId, OrderDto order, CancellationToken cancellationToken)
	{
		if (_appSettings.PaymentSettings.AllPaymentsDisabled)
		{
			await _telegramBot.SendTextMessageAsync(chatId, await _applicationContentStore.GetValueAsync(ApplicationContentKey.Payment.NoEnabledPaymentMethods, CancellationToken.None));
			return;
		}

		var paymentMethodButtons = _paymentTelegramButtonGenerators
			.Where(buttonGenerator => buttonGenerator.PaymentMethodEnabled())
			.Select(async buttonGenerator => new List<InlineKeyboardButton>() { await buttonGenerator.GetInvoiceGenerationButtonAsync(cancellationToken) })
			.Select(task => task.Result);

		InlineKeyboardMarkup inlineKeyboard = new(paymentMethodButtons);

		var message = new StringBuilder();
		var currencySymbol = _currencyCodeToSymbolMapper.GetCurrencySymbol(_appSettings.PaymentSettings.MainCurrency);

		message
			.AppendLine($"{await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.OrderSummary, CancellationToken.None)}")
			.AppendLine(new string('~', 20));

		foreach (var orderCartItem in order.CartItems)
		{
			message
				.AppendLine(orderCartItem.GetFormattedMessage(currencySymbol));
		};

		message
			.AppendLine()
			.AppendLine($"{await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.TotalPrice, CancellationToken.None)}: {order.TotalPrice}{currencySymbol}")
			.AppendLine(new string('~', 20));

		message
			.AppendLine(await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.ChoosePaymentMethod, CancellationToken.None));

		await _telegramBot.SendTextMessageAsync(
			chatId: chatId,
			text: message.ToString(),
			parseMode: ParseMode.Html,
			replyMarkup: inlineKeyboard,
			cancellationToken: cancellationToken);
	}
}
