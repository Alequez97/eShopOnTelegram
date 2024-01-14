using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Keys;
using eShopOnTelegram.Translations.Constants;
using eShopOnTelegram.Translations.Interfaces;
using eShopOnTelegram.Utils.Configuration;

using Newtonsoft.Json;

namespace eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Content;

public class DefaultApplicationContentModel
{
	private readonly ITranslationsService _translationsService;
	private readonly AppSettings _appSettings;

	public DefaultApplicationContentModel(
		ITranslationsService translationsService,
		AppSettings appSettings)
	{
		_translationsService = translationsService;
		_appSettings = appSettings;
	}

	// TELEGRAM BOT
	[JsonProperty(ApplicationContentKey.TelegramBot.WelcomeText)]
	public string TelegramBot_WelcomeText =>
		_translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.WelcomeToOurShop, CancellationToken.None).Result;

	[JsonProperty(ApplicationContentKey.TelegramBot.UnknownCommandText)]
	public string TelegramBot_UnknownCommandText =>
		_translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.UnknownCommand, CancellationToken.None).Result;

	[JsonProperty(ApplicationContentKey.TelegramBot.DefaultErrorMessage)]
	public string TelegramBot_DefaultErrorMessage =>
		_translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.DefaultErrorMessage, CancellationToken.None).Result;

	// PAYMENT
	[JsonProperty(ApplicationContentKey.Payment.SuccessfullPayment)]
	public string Payment_SuccessfullPayment =>
		_translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.ThankYouForPurchase, CancellationToken.None).Result;

	// TODO: ADD SHOP ADMINISTRATOR NAME (LINK)
	[JsonProperty(ApplicationContentKey.Payment.PaymentThroughSeller)]
	public string Payment_PaymentThroughSeller =>
		_translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.PaymentThroughSeller, CancellationToken.None).Result;
}
