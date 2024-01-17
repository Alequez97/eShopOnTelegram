using eShopOnTelegram.Shop.Worker.Commands.Interfaces;
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
	private readonly ChoosePaymentMethodSender _choosePaymentMethodSender;
	private readonly AppSettings _appSettings;

	public ShowActiveOrderCommand(
		ITelegramBotClient telegramBot,
		IOrderService orderService,
		ITranslationsService translationsService,
		ChoosePaymentMethodSender choosePaymentMethodSender,
		AppSettings appSettings)
	{
		_telegramBot = telegramBot;
		_orderService = orderService;
		_translationsService = translationsService;
		_choosePaymentMethodSender = choosePaymentMethodSender;
		_appSettings = appSettings;
	}

	public async Task SendResponseAsync(Update update)
	{
		// TODO:
		// Refactor this command to send available payment methods only when it is not selected
		// When payment method is selected send invoice (or message to pay for generated invoice
		// if for some reason it's impossible to send invoice again)
		// for selected payment method

		var chatId = update.Message.Chat.Id;

		var getOrdersResponse = await _orderService.GetUnpaidOrderByTelegramIdAsync(chatId, CancellationToken.None);

		if (getOrdersResponse.Data != null)
		{
			await _choosePaymentMethodSender.SendAvailablePaymentMethods(chatId, getOrdersResponse.Data, CancellationToken.None);
		}
		else
		{
			await _telegramBot.SendTextMessageAsync(chatId, await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.UnpaidOrderNotFound, CancellationToken.None));
		}
	}

	public async Task<bool> IsResponsibleForUpdateAsync(Update update)
	{
		return update.Message?.Text?.Contains(await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.ShowUnpaidOrder, CancellationToken.None)) ?? false;
	}
}
