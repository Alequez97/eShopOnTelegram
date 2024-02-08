using eShopOnTelegram.Domain.Dto.Orders;
using eShopOnTelegram.Shop.Worker.Services.Telegram.Buttons.Inline.Payment.Interfaces;
using eShopOnTelegram.Shop.Worker.Services.Telegram.MessageSenders.Orders;
using eShopOnTelegram.Translations.Constants;
using eShopOnTelegram.Translations.Interfaces;
using eShopOnTelegram.Utils.Configuration;

using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.Shop.Worker.Services.Telegram.MessageSenders.Payments;

public class ChoosePaymentMethodSender
{
	private readonly ITelegramBotClient _telegramBot;
	private readonly IEnumerable<IPaymentTelegramButtonProvider> _paymentTelegramButtonGenerators;
	private readonly OrderSummarySender _orderSummarySender;
	private readonly ITranslationsService _translationsService;
	private readonly AppSettings _appSettings;

	public ChoosePaymentMethodSender(
		ITelegramBotClient telegramBot,
		IEnumerable<IPaymentTelegramButtonProvider> paymentTelegramButtonGenerators,
		OrderSummarySender orderSummarySender,
		ITranslationsService translationsService,
		AppSettings appSettings)
	{
		_telegramBot = telegramBot;
		_paymentTelegramButtonGenerators = paymentTelegramButtonGenerators;
		_orderSummarySender = orderSummarySender;
		_translationsService = translationsService;
		_appSettings = appSettings;
	}

	public async Task SendAvailablePaymentMethods(long chatId, OrderDto order, CancellationToken cancellationToken)
	{
		if (_appSettings.PaymentSettings.AllPaymentsDisabled)
		{
			await _telegramBot.SendTextMessageAsync(chatId, await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.NoEnabledPaymentMethods, cancellationToken));
			return;
		}

		await _orderSummarySender.SendOrderSummaryAsync(chatId, order, cancellationToken);

		var paymentMethodButtons = _paymentTelegramButtonGenerators
			.Where(buttonGenerator => buttonGenerator.PaymentMethodEnabled())
			.Select(async buttonGenerator => new List<InlineKeyboardButton>() { await buttonGenerator.GetInvoiceGenerationButtonAsync(cancellationToken) })
			.Select(task => task.Result);

		await _telegramBot.SendTextMessageAsync(
			chatId: chatId,
			text: await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.ChoosePaymentMethod, cancellationToken),
			parseMode: ParseMode.Html,
			replyMarkup: new InlineKeyboardMarkup(paymentMethodButtons),
			cancellationToken: cancellationToken
		);
	}
}
