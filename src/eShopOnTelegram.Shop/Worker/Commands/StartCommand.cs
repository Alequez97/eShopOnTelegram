using Ardalis.GuardClauses;

using eShopOnTelegram.Domain.Requests.Customers;
using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Keys;
using eShopOnTelegram.Shop.Worker.Commands.Interfaces;
using eShopOnTelegram.Shop.Worker.Constants;
using eShopOnTelegram.Shop.Worker.Extensions;
using eShopOnTelegram.Shop.Worker.Services.Telegram;
using eShopOnTelegram.Translations.Constants;
using eShopOnTelegram.Translations.Interfaces;
using eShopOnTelegram.Utils.Configuration;

namespace eShopOnTelegram.Shop.Worker.Commands;

public class StartCommand : ITelegramCommand
{
	private readonly ITelegramBotClient _telegramBot;
	private readonly ILogger<StartCommand> _logger;
	private readonly AppSettings _appSettings;
	private readonly ITranslationsService _translationsService;
	private readonly IApplicationContentStore _applicationContentStore;
	private readonly ICustomerService _customerService;
	private readonly IOrderService _orderService;
	private readonly PaymentProceedMessageSender _paymentMethodsSender;

	public StartCommand(
		ITelegramBotClient telegramBot,
		ILogger<StartCommand> logger,
		AppSettings appSettings,
		ITranslationsService translationsService,
		IApplicationContentStore applicationContentStore,
		ICustomerService customerService,
		IOrderService orderService,
		PaymentProceedMessageSender paymentMethodsSender)
	{
		_telegramBot = telegramBot;
		_logger = logger;
		_translationsService = translationsService;
		_appSettings = appSettings;
		_applicationContentStore = applicationContentStore;
		_customerService = customerService;
		_orderService = orderService;
		_paymentMethodsSender = paymentMethodsSender;
	}

	public async Task SendResponseAsync(Update update)
	{
		try
		{
			Guard.Against.Null(update.Message);
			Guard.Against.Null(update.Message.From);

			var createCustomerRequest = new CreateCustomerRequest()
			{
				TelegramUserUID = update.Message.From.Id,
				Username = update.Message.From.Username,
				FirstName = update.Message.From.FirstName,
				LastName = update.Message.From.LastName
			};

			var createCustomerResponse = await _customerService.CreateIfNotPresentAsync(createCustomerRequest);
			var chatId = update.Message.Chat.Id;

			if (createCustomerResponse.Status != ResponseStatus.Success)
			{
				_logger.LogError("Unable to persist new customer.");

				await _telegramBot.SendTextMessageAsync(
					chatId,
					await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.Error_TryAgainLater, CancellationToken.None),
					parseMode: ParseMode.Html
				);

				return;
			}

			var keyboardMarkupBuilder = new KeyboardButtonsMarkupBuilder()
				.AddButtonToCurrentRow(await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.OpenShop, CancellationToken.None), new WebAppInfo() { Url = _appSettings.TelegramBotSettings.WebAppUrl });

			var getOrdersResponse = await _orderService.GetUnpaidOrderByTelegramIdAsync(chatId, CancellationToken.None);
			if (getOrdersResponse.Status == ResponseStatus.Success)
			{
				var activeOrder = getOrdersResponse.Data;

				if (activeOrder != null)
				{
					keyboardMarkupBuilder
						.StartNewRow()
						.AddButtonToCurrentRow(await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.ShowUnpaidOrder, CancellationToken.None));
				}
			}

			await _telegramBot.SendTextMessageAsync(
				chatId,
				await _applicationContentStore.GetValueAsync(ApplicationContentKey.TelegramBot.WelcomeText, CancellationToken.None),
				parseMode: ParseMode.Html,
				replyMarkup: keyboardMarkupBuilder.Build(resizeKeyboard: true)
			);
		}
		catch (Exception exception)
		{
			var chatId = update.Message.Chat.Id;

			_logger.LogError(exception, exception.Message);
			await _telegramBot.SendDefaultErrorMessageAsync(chatId, _applicationContentStore, _logger, CancellationToken.None);
		}
	}

	public Task<bool> IsResponsibleForUpdateAsync(Update update)
	{
		return Task.FromResult(update.Message?.Text?.Contains(CommandConstants.Start) ?? false);
	}
}
