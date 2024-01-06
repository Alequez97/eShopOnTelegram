using eShopOnTelegram.Shop.Worker.Commands.Interfaces;

namespace eShopOnTelegram.Shop.Worker.Commands.Groups;

public class ChatMemberAddedCommand : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBot;

    public ChatMemberAddedCommand(
        ITelegramBotClient telegramBot
        )
    {
        _telegramBot = telegramBot;
    }

    public async Task SendResponseAsync(Update update)
    {
        if (update.Message.NewChatMembers.Length == 1)
        {
            var welcomeMessage = $"Welcome new person on the board! @{update.Message.NewChatMembers[0].Username ?? update.Message.NewChatMembers[0].FirstName}";

            await _telegramBot.SendTextMessageAsync(
                update.Message.Chat.Id,
                welcomeMessage,
                parseMode: ParseMode.Html);

            return;
        }
    }

    public Task<bool> IsResponsibleForUpdateAsync(Update update)
    {
        return Task.FromResult(update.Message?.Type == MessageType.ChatMembersAdded);
    }
}
