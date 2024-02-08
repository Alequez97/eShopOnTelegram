using eShopOnTelegram.Domain.Dto.Orders;
using eShopOnTelegram.Persistence.Entities.Payments;
using eShopOnTelegram.Shop.Worker.Services.Telegram.MessageSenders.Payments.Invoices.Interfaces;
using eShopOnTelegram.Translations.Constants;
using eShopOnTelegram.Translations.Interfaces;
using eShopOnTelegram.Utils.Configuration;

using Telegram.Bot.Types.ReplyMarkups;
namespace eShopOnTelegram.Shop.Worker.Services.Telegram.MessageSenders.Payments.Invoices;

public class CoinGateInvoiceSender : IInvoiceSender
{
	private readonly ITelegramBotClient _telegramBot;
	private readonly ITranslationsService _translationsService;
	private readonly AppSettings _appSettings;

	public CoinGateInvoiceSender(
		ITelegramBotClient telegramBot,
		ITranslationsService translationsService,
		AppSettings appSettings)
	{
		_telegramBot = telegramBot;
		_translationsService = translationsService;
		_appSettings = appSettings;
	}

	public async Task SendInvoiceAsync(long telegramId, OrderDto orderDto, CancellationToken cancellationToken)
	{
		var buttonText = await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.ProceedToPayment, CancellationToken.None);
		InlineKeyboardMarkup inlineKeyboard = new(new[]
		{
			// first row
				new []
				{
					InlineKeyboardButton.WithUrl(buttonText, orderDto.InvoiceUrl),
				},
			});

		await _telegramBot.SendTextMessageAsync(
			chatId: telegramId,
			text: await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.InvoiceReceived, CancellationToken.None),
			replyMarkup: inlineKeyboard,
			cancellationToken: CancellationToken.None);
	}

	public PaymentMethod PaymentMethod => PaymentMethod.CoinGate;
}
