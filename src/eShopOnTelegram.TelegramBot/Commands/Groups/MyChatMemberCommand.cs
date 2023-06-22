using eshopOnTelegram.TelegramBot.Appsettings;

using eShopOnTelegram.TelegramBot.Commands.Interfaces;

namespace eShopOnTelegram.TelegramBot.Commands.Groups;

/// <summary>
/// Command that is executed when bot is added to telegram group
/// </summary>
public class MyChatMemberCommand : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBot;
    private readonly TelegramAppsettings _telegramAppsettings;

    public MyChatMemberCommand(
        ITelegramBotClient telegramBot,
        TelegramAppsettings telegramAppsettings
        )
    {
        _telegramBot = telegramBot;
        _telegramAppsettings = telegramAppsettings;
    }

    public async Task SendResponseAsync(Update update)
    {
        if (string.Equals(update.MyChatMember.From.Id.ToString(), _telegramAppsettings.BotOwnerTelegramId, StringComparison.OrdinalIgnoreCase))
        {
            // TODO: Persist id of the chat where notifications will be send

            await _telegramBot.SendTextMessageAsync(update.MyChatMember.Chat.Id, "You are owner of the bot");
        }
    }

    public bool IsResponsibleForUpdate(Update update)
    {
        return update.Type == UpdateType.MyChatMember;
    }
}