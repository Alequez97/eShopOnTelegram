using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Translations.Constants;
using eShopOnTelegram.Translations.Interfaces;
using eShopOnTelegram.Utils.Configuration;

using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.Shop.Worker.Services.Telegram.Buttons.Keyboard;

public class OpenShopKeyboardButtonsLayoutProvider
{
	private readonly IOrderService _orderService;
	private readonly ITranslationsService _translationsService;
	private readonly AppSettings _appSettings;

	public OpenShopKeyboardButtonsLayoutProvider(
		IOrderService orderService,
		ITranslationsService translationsService,
		AppSettings appSettings)
	{
		_orderService = orderService;
		_translationsService = translationsService;
		_appSettings = appSettings;
	}

	public async Task<ReplyKeyboardMarkup> GetOpenShopKeyboardLayoutAsync(long telegramId, CancellationToken cancellationToken)
	{
		var openShopButtonText = await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.OpenShop, cancellationToken);

		var keyboardMarkupBuilder = new KeyboardButtonsLayoutBuilder();

		if (_appSettings.TelegramBotSettings.HasWebAppLayout)
		{
			var webAppInfo = new WebAppInfo()
			{
				Url = _appSettings.TelegramBotSettings.ShopAppUrl
			};

			keyboardMarkupBuilder.AddButtonToCurrentRow(openShopButtonText, webAppInfo);
		}
		else
		{
			keyboardMarkupBuilder.AddButtonToCurrentRow(openShopButtonText);
		}

		var getOrdersResponse = await _orderService.GetUnpaidOrderByTelegramIdAsync(telegramId, cancellationToken);
		if (getOrdersResponse.Status == ResponseStatus.Success)
		{
			var activeOrder = getOrdersResponse.Data;
			if (activeOrder != null)
			{
				var showUnpaidOrderButtonText = await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.ShowUnpaidOrder, cancellationToken);

				keyboardMarkupBuilder
					.StartNewRow()
					.AddButtonToCurrentRow(showUnpaidOrderButtonText);
			}
		}

		return keyboardMarkupBuilder.Build(resizeKeyboard: true);
	}
}
