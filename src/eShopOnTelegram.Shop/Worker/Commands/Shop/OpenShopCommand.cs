using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.Shop.Worker.Commands.Interfaces;
using eShopOnTelegram.Shop.Worker.Constants;
using eShopOnTelegram.Shop.Worker.Extensions;
using eShopOnTelegram.Translations.Constants;
using eShopOnTelegram.Translations.Interfaces;
using eShopOnTelegram.Utils.Configuration;

using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.Shop.Worker.Commands.Shop;

/// <summary>
/// This command will be triggered for customers that does not uses WebApp layout
/// </summary>
public class OpenShopCommand : ITelegramCommand
{
	private readonly ITelegramBotClient _telegramBot;
	private readonly ITranslationsService _translationsService;
	private readonly IApplicationContentStore _applicationContentStore;
	private readonly IProductCategoryService _productCategoryService;
	private readonly AppSettings _appSettings;
	private readonly ILogger<OpenShopCommand> _logger;

	public OpenShopCommand(
		ITelegramBotClient telegramBot,
		ITranslationsService translationsService,
		IApplicationContentStore applicationContentStore,
		IProductCategoryService productCategoryService,
		AppSettings appSettings,
		ILogger<OpenShopCommand> logger)
	{
		_telegramBot = telegramBot;
		_translationsService = translationsService;
		_applicationContentStore = applicationContentStore;
		_productCategoryService = productCategoryService;
		_appSettings = appSettings;
		_logger = logger;
	}

	public async Task SendResponseAsync(Update update)
	{
		try
		{
			var chatId = update.Message.Chat.Id;

			var getAllProductCategoriesResponse = await _productCategoryService.GetAllAsync(CancellationToken.None);

			if (getAllProductCategoriesResponse.Status != ResponseStatus.Success)
			{
				await _telegramBot.SendDefaultErrorMessageAsync(chatId, _applicationContentStore, _logger, CancellationToken.None);
				return;
			}

			InlineKeyboardMarkup inlineKeyboard = new(
				getAllProductCategoriesResponse.Data
					.Select(productCategory => new[] {
						InlineKeyboardButton
							.WithCallbackData(text: $"{productCategory.Name}", callbackData: $"{InlineButtonCallbackQueryData.ShowProducts}{InlineButtonCallbackQueryData.DataSeparator}{productCategory.Name}")
					})
					.ToArray()
			);

			await _telegramBot.SendTextMessageAsync(
				chatId,
				await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.SelectCategory, CancellationToken.None)	,
				parseMode: ParseMode.Html,
				replyMarkup: inlineKeyboard);
		}
		catch (Exception exception)
		{
			var chatId = update.Message.Chat.Id;

			_logger.LogError(exception, exception.Message);
			await _telegramBot.SendDefaultErrorMessageAsync(chatId, _applicationContentStore, _logger, CancellationToken.None);
		}
	}

	public async Task<bool> IsResponsibleForUpdateAsync(Update update)
	{
		var openShopButtonText = await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.OpenShop, CancellationToken.None);
		return update.Message?.Text?.Contains(openShopButtonText) ?? false;
	}
}
