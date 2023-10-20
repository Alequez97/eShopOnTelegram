using eShopOnTelegram.RuntimeConfiguration.BotOwnerData.Interfaces;
using eShopOnTelegram.TelegramBot.Worker.Commands.Interfaces;

namespace eShopOnTelegram.TelegramBot.Worker.Commands.Groups;

/// <summary>
/// Command that is executed when bot is added to telegram group
/// </summary>
public class MyChatMemberCommand : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBot;
    private readonly TelegramBotSettings _telegramBotSettings;
    private readonly IBotOwnerDataStore _botOwnerDataStore;

    public MyChatMemberCommand(
        ITelegramBotClient telegramBot,
        AppSettings appSettings,
        IBotOwnerDataStore botOwnerDataStore
        )
    {
        _telegramBot = telegramBot;
        _telegramBotSettings = appSettings.TelegramBotSettings;
        _botOwnerDataStore = botOwnerDataStore;
    }

    public async Task SendResponseAsync(Update update)
    {
        if (string.Equals(update.MyChatMember.From.Id.ToString(), _telegramBotSettings.BotOwnerTelegramId, StringComparison.OrdinalIgnoreCase))
        {
            var groupOwnerTelegramGroupId = await _botOwnerDataStore.GetBotOwnerTelegramGroupIdAsync(CancellationToken.None);
            if (string.IsNullOrWhiteSpace(groupOwnerTelegramGroupId))
            {
                await _botOwnerDataStore.SaveBotOwnerTelegramGroupIdAsync(update.MyChatMember.Chat.Id.ToString(), CancellationToken.None);

                var welcomeMessage = $"Hello. If you see this message, that means you are owner of this group and bot @{update.MyChatMember.NewChatMember.User.Username}. \nYou will get notification when you will receive new payments for orders. Be aware and check that notifications are turned on for this group \nGood luck and in case of some problems please contact developer of this bot - @Alequez97";

                await _telegramBot.SendTextMessageAsync(
                    update.MyChatMember.Chat.Id,
                    welcomeMessage,
                    parseMode: ParseMode.Html);
            }
        }
    }

    public Task<bool> IsResponsibleForUpdateAsync(Update update)
    {
        return Task.FromResult(update.Type == UpdateType.MyChatMember);
    }
}