using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Keys;
using eShopOnTelegram.Shop.Worker.Commands.Interfaces;
using eShopOnTelegram.Shop.Worker.Services.Telegram;
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
	private readonly PaymentProceedMessageSender _paymentProceedMessage;
	private readonly AppSettings _appSettings;

	public ShowActiveOrderCommand(
		ITelegramBotClient telegramBot,
		IOrderService orderService,
		ITranslationsService translationsService,
		IApplicationContentStore applicationContentStore,
		PaymentProceedMessageSender paymentProceedMessageSender,
		AppSettings appSettings)
	{
		_telegramBot = telegramBot;
		_orderService = orderService;
		_translationsService = translationsService;
		_applicationContentStore = applicationContentStore;
		_paymentProceedMessage = paymentProceedMessageSender;
		_appSettings = appSettings;
	}

	public async Task SendResponseAsync(Update update)
	{
		var chatId = update.Message.Chat.Id;

		var getOrdersResponse = await _orderService.GetUnpaidOrderByTelegramIdAsync(chatId, CancellationToken.None);

		if (getOrdersResponse.Data != null)
		{
			await _paymentProceedMessage.SendProceedToPaymentAsync(chatId, getOrdersResponse.Data, CancellationToken.None);
		}
		else
		{
			await _telegramBot.SendTextMessageAsync(chatId, await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.UnpaidOrderNotFound, CancellationToken.None));
		}
	}

	public async Task<bool> IsResponsibleForUpdateAsync(Update update)
	{
		return update.Message?.Text?.Contains(await _applicationContentStore.GetValueAsync(ApplicationContentKey.Order.ShowUnpaidOrder, CancellationToken.None)) ?? false;
	}
}
