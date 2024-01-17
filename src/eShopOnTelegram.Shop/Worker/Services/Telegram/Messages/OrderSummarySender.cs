using System.Text;

using eShopOnTelegram.Domain.Dto.Orders;
using eShopOnTelegram.Shop.Worker.Services.Mappers;
using eShopOnTelegram.Translations.Constants;
using eShopOnTelegram.Translations.Interfaces;
using eShopOnTelegram.Utils.Configuration;

namespace eShopOnTelegram.Shop.Worker.Services.Telegram.Messages;

public class OrderSummarySender
{
	private readonly ITelegramBotClient _telegramBot;
	private readonly ITranslationsService _translationsService;
	private readonly AppSettings _appSettings;
	private readonly CurrencyCodeToSymbolMapper _currencyCodeToSymbolMapper;

	public OrderSummarySender(
		ITelegramBotClient telegramBot,
		ITranslationsService translationsService,
		AppSettings appSettings,
		CurrencyCodeToSymbolMapper currencyCodeToSymbolMapper)
	{
		_telegramBot = telegramBot;
		_translationsService = translationsService;
		_appSettings = appSettings;
		_currencyCodeToSymbolMapper = currencyCodeToSymbolMapper;
	}

	public async Task SendOrderSummaryAsync(long telegramId, OrderDto order, CancellationToken cancellationToken)
	{
		var message = new StringBuilder();
		var currencySymbol = _currencyCodeToSymbolMapper.GetCurrencySymbol(_appSettings.PaymentSettings.MainCurrency);

		message
			.AppendLine($"{await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.OrderSummary, cancellationToken)}")
			.AppendLine(new string('~', 20));

		foreach (var orderCartItem in order.CartItems)
		{
			message.AppendLine(orderCartItem.GetFormattedMessage(currencySymbol));
		};

		message
			.AppendLine()
			.AppendLine(new string('~', 20))
			.AppendLine($"{await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.TotalPrice, cancellationToken)}: {order.TotalPrice}{currencySymbol}");

		await _telegramBot.SendTextMessageAsync(
			chatId: telegramId,
			text: message.ToString(),
			parseMode: ParseMode.Html,
			cancellationToken: cancellationToken
		);
	}
}
