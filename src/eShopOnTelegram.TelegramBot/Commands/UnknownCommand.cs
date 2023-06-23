using eShopOnTelegram.ApplicationContent.Interfaces;
using eShopOnTelegram.ApplicationContent.Keys;
using eShopOnTelegram.TelegramBot.Commands.Interfaces;

namespace eShopOnTelegram.TelegramBot.Commands;

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