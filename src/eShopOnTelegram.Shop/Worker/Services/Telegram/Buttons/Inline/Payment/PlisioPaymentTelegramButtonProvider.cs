using eShopOnTelegram.Shop.Worker.Constants;
using eShopOnTelegram.Shop.Worker.Services.Telegram.Buttons.Inline.Payment.Interfaces;
using eShopOnTelegram.Translations.Constants;
using eShopOnTelegram.Translations.Interfaces;
using eShopOnTelegram.Utils.Configuration;

using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.Shop.Worker.Services.Telegram.Buttons.Inline.Payment;

public class PlisioPaymentTelegramButtonProvider : IPaymentTelegramButtonProvider
{
	private readonly ITranslationsService _translationsService;
	private readonly AppSettings _appSettings;

	public PlisioPaymentTelegramButtonProvider(
		ITranslationsService translationsService,
		AppSettings appSettings)
	{
		_translationsService = translationsService;
		_appSettings = appSettings;
	}

	public async Task<InlineKeyboardButton> GetInvoiceGenerationButtonAsync(CancellationToken cancellationToken)
	{
		var buttonText = await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.PayWithCrypto, cancellationToken);
		var button = InlineKeyboardButton.WithCallbackData(text: buttonText, callbackData: PaymentMethodConstants.Plisio);

		return await Task.FromResult(button);
	}

	public bool PaymentMethodEnabled()
	{
		return _appSettings.PaymentSettings.Plisio.Enabled;
	}
}
