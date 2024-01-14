using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Keys;
using eShopOnTelegram.Shop.Worker.Commands.Interfaces;
using eShopOnTelegram.Shop.Worker.Services.Telegram.Buttons.Keyboard;

namespace eShopOnTelegram.Shop.Worker.Commands;

public class UnknownCommand : ITelegramCommand
{
	private readonly ITelegramBotClient _telegramBot;
	private readonly IApplicationContentStore _applicationContentStore;
	private readonly OpenShopKeyboardButtonsLayoutProvider _openShopKeyboardButtonsLayoutProvider;

	public UnknownCommand(
		ITelegramBotClient telegramBot,
		IApplicationContentStore applicationContentStore,
		OpenShopKeyboardButtonsLayoutProvider openShopKeyboardButtonsLayoutProvider)
	{
		_telegramBot = telegramBot;
		_applicationContentStore = applicationContentStore;
		_openShopKeyboardButtonsLayoutProvider = openShopKeyboardButtonsLayoutProvider;
	}

	public async Task SendResponseAsync(Update update)
	{
		var chatId = update?.Message?.Chat?.Id;

		if (chatId.HasValue)
		{
			await _telegramBot.SendTextMessageAsync(
				chatId,
				await _applicationContentStore.GetValueAsync(ApplicationContentKey.TelegramBot.UnknownCommandText, CancellationToken.None),
				parseMode: ParseMode.Html,
				replyMarkup: await _openShopKeyboardButtonsLayoutProvider.GetOpenShopKeyboardLayoutAsync(chatId.Value, CancellationToken.None)
			);
		}
	}

	public Task<bool> IsResponsibleForUpdateAsync(Update update)
	{
		return Task.FromResult(false);
	}
}
