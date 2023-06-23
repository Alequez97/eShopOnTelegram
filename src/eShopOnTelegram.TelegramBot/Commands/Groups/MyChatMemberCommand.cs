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

            var welcomeMessage = $"Hello. If you see this message, that means you are owner of this group and bot @{update.MyChatMember.NewChatMember.User.Username}. \nYou will get notification when you will receive new payments for orders. Be aware and check that notifications are turned on for this group \nGood luck and in case of some problems please contact developer of this bot - @Alequez97";

            await _telegramBot.SendTextMessageAsync(
                update.MyChatMember.Chat.Id,
                welcomeMessage,
                parseMode: ParseMode.Html);
        }
    }

    public Task<bool> IsResponsibleForUpdateAsync(Update update)
    {
        return Task.FromResult(update.Type == UpdateType.MyChatMember);
    }
}