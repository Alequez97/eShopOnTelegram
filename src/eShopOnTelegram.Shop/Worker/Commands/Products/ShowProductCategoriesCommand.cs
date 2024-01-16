using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.Shop.Worker.Commands.Interfaces;
using eShopOnTelegram.Shop.Worker.Constants;
using eShopOnTelegram.Shop.Worker.Extensions;
using eShopOnTelegram.Translations.Constants;
using eShopOnTelegram.Translations.Interfaces;
using eShopOnTelegram.Utils.Configuration;

using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.Shop.Worker.Commands.Products;
public class ShowProductCategoriesCommand : ITelegramCommand
{
	private readonly ITelegramBotClient _telegramBot;
	private readonly IProductCategoryService _productCategoryService;
	private readonly ITranslationsService _translationsService;
	private readonly IApplicationContentStore _applicationContentStore;
	private readonly AppSettings _appSettings;
	private readonly ILogger<ShowProductCategoriesCommand> _logger;

	public ShowProductCategoriesCommand(
		ITelegramBotClient telegramBot,
		IProductCategoryService productCategoryService,
		ITranslationsService translationsService,
		IApplicationContentStore applicationContentStore,
		AppSettings appSettings,
		ILogger<ShowProductCategoriesCommand> logger)
	{
		_telegramBot = telegramBot;
		_productCategoryService = productCategoryService;
		_translationsService = translationsService;
		_applicationContentStore = applicationContentStore;
		_appSettings = appSettings;
		_logger = logger;
	}

	public async Task SendResponseAsync(Update update)
	{
		var chatId = update.CallbackQuery.From.Id;

		try
		{
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

			await _telegramBot.EditMessageTextAsync(
			chatId,
			update.CallbackQuery.Message.MessageId,
			await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.SelectCategory, CancellationToken.None),
			parseMode: ParseMode.Html,
				replyMarkup: inlineKeyboard);
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, exception.Message);
			await _telegramBot.SendDefaultErrorMessageAsync(chatId, _applicationContentStore, _logger, CancellationToken.None);
		}
	}

	public Task<bool> IsResponsibleForUpdateAsync(Update update)
	{
		return Task.FromResult(update.Type == UpdateType.CallbackQuery && update.CallbackQuery.Data.Contains(InlineButtonCallbackQueryData.ShowProductCategories));
	}
}
