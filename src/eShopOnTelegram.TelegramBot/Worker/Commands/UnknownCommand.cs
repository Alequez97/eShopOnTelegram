using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Keys;
using eShopOnTelegram.TelegramBot.Worker.Commands.Interfaces;

namespace eShopOnTelegram.TelegramBot.Worker.Commands;

public class UnknownCommand : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBot;
    private readonly IApplicationContentStore _applicationContentStore;

    public UnknownCommand(ITelegramBotClient telegramBot, IApplicationContentStore applicationContentStore)
    {
        _telegramBot = telegramBot;
        _applicationContentStore = applicationContentStore;
    }

    public async Task SendResponseAsync(Update update)
    {
        var chatId = update?.Message?.Chat?.Id;

        if (chatId != null)
        {
            await _telegramBot.SendTextMessageAsync(
                chatId,
                await _applicationContentStore.GetValueAsync(ApplicationContentKey.TelegramBot.UnknownCommandText, CancellationToken.None),
                parseMode: ParseMode.Html
            );
        }
    }

    public Task<bool> IsResponsibleForUpdateAsync(Update update)
    {
        return Task.FromResult(false);
    }
}