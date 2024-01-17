using System.Text;

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

public class ShowQuantitySelectorCommand : ITelegramCommand
{
	private readonly ITelegramBotClient _telegramBot;
	private readonly IProductAttributeService _productAttributeService;
	private readonly ITranslationsService _translationsService;
	private readonly IApplicationContentStore _applicationContentStore;
	private readonly AppSettings _appSettings;
	private readonly ILogger<ShowQuantitySelectorCommand> _logger;

	public ShowQuantitySelectorCommand(
		ITelegramBotClient telegramBot,
		IProductAttributeService productAttributeService,
		ITranslationsService translationsService,
		IApplicationContentStore applicationContentStore,
		AppSettings appSettings,
		ILogger<ShowQuantitySelectorCommand> logger)
	{
		_telegramBot = telegramBot;
		_productAttributeService = productAttributeService;
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
			var selectedProductCategoryName = update.CallbackQuery.Data.Split(InlineButtonCallbackQueryData.DataSeparator)[1];
			var selectedProductAttributeId = update.CallbackQuery.Data.Split(InlineButtonCallbackQueryData.DataSeparator)[2];

			var getSelectedProductResponse = await _productAttributeService.GetAsync(Convert.ToInt64(selectedProductAttributeId), CancellationToken.None);

			if (getSelectedProductResponse.Status != ResponseStatus.Success)
			{
				_logger.LogError("[ShowQuantitySelectorCommand] Failed to get product attribute by id");
				await _telegramBot.SendDefaultErrorMessageAsync(chatId, _applicationContentStore, _logger, CancellationToken.None);
			}

			InlineKeyboardMarkup inlineKeyboard = new(new[]
			{
				// first row
				new []
				{
					InlineKeyboardButton.WithCallbackData("1", $"{InlineButtonCallbackQueryData.ShowAvailablePaymentMethods}{InlineButtonCallbackQueryData.DataSeparator}{selectedProductAttributeId}{InlineButtonCallbackQueryData.DataSeparator}1"),
					InlineKeyboardButton.WithCallbackData("2", $"{InlineButtonCallbackQueryData.ShowAvailablePaymentMethods}{InlineButtonCallbackQueryData.DataSeparator}{selectedProductAttributeId}{InlineButtonCallbackQueryData.DataSeparator}2"),
					InlineKeyboardButton.WithCallbackData("3", $"{InlineButtonCallbackQueryData.ShowAvailablePaymentMethods}{InlineButtonCallbackQueryData.DataSeparator}{selectedProductAttributeId}{InlineButtonCallbackQueryData.DataSeparator}3"),
					InlineKeyboardButton.WithCallbackData("4", $"{InlineButtonCallbackQueryData.ShowAvailablePaymentMethods}{InlineButtonCallbackQueryData.DataSeparator}{selectedProductAttributeId}{InlineButtonCallbackQueryData.DataSeparator}4"),
					InlineKeyboardButton.WithCallbackData("5", $"{InlineButtonCallbackQueryData.ShowAvailablePaymentMethods}{InlineButtonCallbackQueryData.DataSeparator}{selectedProductAttributeId}{InlineButtonCallbackQueryData.DataSeparator}5"),
				},
				// second row
				new []
				{
					InlineKeyboardButton.WithCallbackData("6", $"{InlineButtonCallbackQueryData.ShowAvailablePaymentMethods}{InlineButtonCallbackQueryData.DataSeparator}{selectedProductAttributeId}{InlineButtonCallbackQueryData.DataSeparator}6"),
					InlineKeyboardButton.WithCallbackData("7", $"{InlineButtonCallbackQueryData.ShowAvailablePaymentMethods}{InlineButtonCallbackQueryData.DataSeparator}{selectedProductAttributeId}{InlineButtonCallbackQueryData.DataSeparator}7"),
					InlineKeyboardButton.WithCallbackData("8", $"{InlineButtonCallbackQueryData.ShowAvailablePaymentMethods}{InlineButtonCallbackQueryData.DataSeparator}{selectedProductAttributeId}{InlineButtonCallbackQueryData.DataSeparator}8"),
					InlineKeyboardButton.WithCallbackData("9", $"{InlineButtonCallbackQueryData.ShowAvailablePaymentMethods}{InlineButtonCallbackQueryData.DataSeparator}{selectedProductAttributeId}{InlineButtonCallbackQueryData.DataSeparator}9"),
					InlineKeyboardButton.WithCallbackData("10", $"{InlineButtonCallbackQueryData.ShowAvailablePaymentMethods}{InlineButtonCallbackQueryData.DataSeparator}{selectedProductAttributeId}{InlineButtonCallbackQueryData.DataSeparator}10"),
				},
				new []
				{
					InlineKeyboardButton.WithCallbackData(await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.BackToProducts, CancellationToken.None), $"{InlineButtonCallbackQueryData.ShowProducts}{InlineButtonCallbackQueryData.DataSeparator}{selectedProductCategoryName}"),
				},
				new []
				{
					InlineKeyboardButton.WithCallbackData(await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.BackToCategories, CancellationToken.None), InlineButtonCallbackQueryData.ShowProductCategories),
				},
			});

			var message = new StringBuilder()
				.AppendLine($"{await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.Category, CancellationToken.None)}: {selectedProductCategoryName}")
				.AppendLine($"{await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.Product, CancellationToken.None)}: {getSelectedProductResponse.Data.ProductName}");
				
			await _telegramBot.EditMessageTextAsync(chatId, update.CallbackQuery.Message.MessageId, message.ToString());

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
		return Task.FromResult(update.Type == UpdateType.CallbackQuery && update.CallbackQuery.Data.Contains(InlineButtonCallbackQueryData.ShowQuantitySelector));
	}
}
