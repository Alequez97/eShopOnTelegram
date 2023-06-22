using eShopOnTelegram.TelegramBot.Appsettings;
using eShopOnTelegram.TelegramBot.Commands.Interfaces;
using eShopOnTelegram.TelegramBot.Constants;
using eShopOnTelegram.TelegramBot.Extensions;

namespace eShopOnTelegram.TelegramBot.Commands;

public class UnknownCommand : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBot;
    private readonly BotContentAppsettings _botContentAppsettings;

    public UnknownCommand(ITelegramBotClient telegramBot, BotContentAppsettings botContentAppsettings)
    {
        _telegramBot = telegramBot;
        _botContentAppsettings = botContentAppsettings;
    }

    public async Task SendResponseAsync(Update update)
    {
        var chatId = update?.Message?.Chat?.Id;

        if (chatId != null)
        {
            await _telegramBot.SendTextMessageAsync(
                chatId,
                _botContentAppsettings.Common.UnknownCommandText.OrNextIfNullOrEmpty(BotContentDefaultConstants.Common.UnknownCommandText),
                parseMode: ParseMode.Html
            );
        }
    }

    public bool IsResponsibleForUpdate(Update update)
    {
        return false;
    }
}