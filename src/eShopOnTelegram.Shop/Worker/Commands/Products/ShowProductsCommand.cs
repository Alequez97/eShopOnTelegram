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

public class ShowProductsCommand : ITelegramCommand
{
	private readonly ITelegramBotClient _telegramBot;
	private readonly IProductService _productService;
	private readonly ITranslationsService _translationsService;
	private readonly IApplicationContentStore _applicationContentStore;
	private readonly AppSettings _appSettings;
	private readonly ILogger<ShowProductsCommand> _logger;

	public ShowProductsCommand(
		ITelegramBotClient telegramBot,
		IProductService productService,
		ITranslationsService translationsService,
		IApplicationContentStore applicationContentStore,
		AppSettings appSettings,
		ILogger<ShowProductsCommand> logger)
	{
		_telegramBot = telegramBot;
		_productService = productService;
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
			var selectedProductCategory = update.CallbackQuery.Data.Split(InlineButtonCallbackQueryData.DataSeparator)[1];

			var getProductsResponse = await _productService.GetAllByCategoryAsync(selectedProductCategory, CancellationToken.None);

			if (getProductsResponse.Status != ResponseStatus.Success)
			{
				_logger.LogError("Unable to get products from categories");

				await _telegramBot.SendTextMessageAsync(
				chatId,
					await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.Error_TryAgainLater, CancellationToken.None),
					parseMode: ParseMode.Html
				);

				return;
			}

			InlineKeyboardMarkup inlineKeyboard = new(
				getProductsResponse.Data
					.Select(product => new[] {
						// As callback data product attribute id is used assuming, that for customers with inline buttons layout there will be only one product attribute in each product
						InlineKeyboardButton
							.WithCallbackData(text: $"{product.Name}", callbackData: $"{InlineButtonCallbackQueryData.ShowQuantitySelector}{InlineButtonCallbackQueryData.DataSeparator}{selectedProductCategory}{InlineButtonCallbackQueryData.DataSeparator}{product.Name}{InlineButtonCallbackQueryData.DataSeparator}{product.ProductAttributes.First().Id}")
					})
					.Append(new[]
					{
						InlineKeyboardButton
						.WithCallbackData(await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.BackToCategories, CancellationToken.None), InlineButtonCallbackQueryData.ShowProductCategories)
					})
					.ToArray()
			);

			await _telegramBot.EditMessageTextAsync(chatId,
				update.CallbackQuery.Message.MessageId, $"{await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.Category, CancellationToken.None)}: {selectedProductCategory}");

			await _telegramBot.EditMessageReplyMarkupAsync(
				chatId,
				update.CallbackQuery.Message.MessageId,
				replyMarkup: inlineKeyboard
			);
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, exception.Message);
			await _telegramBot.SendDefaultErrorMessageAsync(chatId, _applicationContentStore, _logger, CancellationToken.None);
		}
	}

	public Task<bool> IsResponsibleForUpdateAsync(Update update)
	{
		return Task.FromResult(update.Type == UpdateType.CallbackQuery && update.CallbackQuery.Data.Contains(InlineButtonCallbackQueryData.ShowProducts));
	}
}
