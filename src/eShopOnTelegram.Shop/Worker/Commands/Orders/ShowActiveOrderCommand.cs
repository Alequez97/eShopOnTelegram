using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.Shop.Worker.Commands.Interfaces;
using eShopOnTelegram.Shop.Worker.Extensions;
using eShopOnTelegram.Shop.Worker.Services.Telegram.Messages;
using eShopOnTelegram.Translations.Constants;
using eShopOnTelegram.Translations.Interfaces;
using eShopOnTelegram.Utils.Configuration;

namespace eShopOnTelegram.Shop.Worker.Commands.Orders;

public class ShowActiveOrderCommand : ITelegramCommand
{
	private readonly ITelegramBotClient _telegramBot;
	private readonly IOrderService _orderService;
	private readonly ITranslationsService _translationsService;
	private readonly IApplicationContentStore _applicationContentStore;
	private readonly ChoosePaymentMethodSender _choosePaymentMethodSender;
	private readonly AppSettings _appSettings;
	private readonly ILogger<ShowActiveOrderCommand> _logger;

	public ShowActiveOrderCommand(
		ITelegramBotClient telegramBot,
		IOrderService orderService,
		ITranslationsService translationsService,
		IApplicationContentStore applicationContentStore,
		ChoosePaymentMethodSender choosePaymentMethodSender,
		AppSettings appSettings,
		ILogger<ShowActiveOrderCommand> logger)
	{
		_telegramBot = telegramBot;
		_orderService = orderService;
		_translationsService = translationsService;
		_applicationContentStore = applicationContentStore;
		_choosePaymentMethodSender = choosePaymentMethodSender;
		_appSettings = appSettings;
		_logger = logger;
	}

	public async Task SendResponseAsync(Update update)
	{
		var chatId = update.Message.Chat.Id;

		try
		{
			// TODO:
			// Refactor this command to send available payment methods only when it is not selected
			// When payment method is selected send invoice (or message to pay for generated invoice
			// if for some reason it's impossible to send invoice again)
			// for selected payment method

			var getOrdersResponse = await _orderService.GetUnpaidOrderByTelegramIdAsync(chatId, CancellationToken.None);

			if (getOrdersResponse.Status == ResponseStatus.NotFound)
			{
				await _telegramBot.SendTextMessageAsync(chatId, await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.UnpaidOrderNotFound, CancellationToken.None));
				return;
			}

			if (getOrdersResponse.Status != ResponseStatus.Success)
			{
				_logger.LogError("[ShowActiveOrderCommand]: Can't get order by telegram id");
				await _telegramBot.SendDefaultErrorMessageAsync(chatId, _applicationContentStore, _logger, CancellationToken.None);
				return;
			}

			var activeOrder = getOrdersResponse.Data;

			if (activeOrder.PaymentMethodSelected)
			{
				// TODO: Send invoice for selected payment method
				// If for some reason it is impossible to send it again write message to pay for previous invoice
				// or create new order
				var message = await _telegramBot.SendTextMessageAsync(chatId, $"PLACEHOLDER: Pay for sent invoice or create new order");
			}
			else
			{
				await _choosePaymentMethodSender.SendAvailablePaymentMethods(chatId, activeOrder, CancellationToken.None);
			}
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, exception.Message);
			await _telegramBot.SendDefaultErrorMessageAsync(chatId, _applicationContentStore, _logger, CancellationToken.None);
		}
	}

	public async Task<bool> IsResponsibleForUpdateAsync(Update update)
	{
		return update.Message?.Text?.Contains(await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.ShowUnpaidOrder, CancellationToken.None)) ?? false;
	}
}
