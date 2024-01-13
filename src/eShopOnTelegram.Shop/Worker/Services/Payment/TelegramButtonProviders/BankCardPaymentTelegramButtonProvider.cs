using eShopOnTelegram.Shop.Worker.Constants;
using eShopOnTelegram.Shop.Worker.Services.Payment.Interfaces;
using eShopOnTelegram.Translations.Constants;
using eShopOnTelegram.Translations.Interfaces;
using eShopOnTelegram.Utils.Configuration;

using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.Shop.Worker.Services.Payment.TelegramButtonProviders;

public class BankCardPaymentTelegramButtonProvider : IPaymentTelegramButtonProvider
{
	private readonly ITranslationsService _translationsService;
	private readonly AppSettings _appSettings;

	public BankCardPaymentTelegramButtonProvider(ITranslationsService translationsService, AppSettings appSettings)
	{
		_translationsService = translationsService;
		_appSettings = appSettings;
	}

	public async Task<InlineKeyboardButton> GetInvoiceGenerationButtonAsync(CancellationToken cancellationToken)
	{
		var buttonText = await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.PayWithBankCard, cancellationToken);
		var button = InlineKeyboardButton.WithCallbackData(text: buttonText, callbackData: PaymentMethodConstants.BankCard);

		return button;
	}

	public bool PaymentMethodEnabled()
	{
		return _appSettings.PaymentSettings.Card.Enabled;
	}
}
