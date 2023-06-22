using System.Text;

using eShopOnTelegram.TelegramBot.Commands.Interfaces;

namespace eShopOnTelegram.TelegramBot.Commands.Groups;

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

        if (update.Message.NewChatMembers.Length > 1)
        {
            var welcomeMessage = new StringBuilder();

            welcomeMessage
                .AppendLine($"Welcome {update.Message.NewChatMembers.Length} new persons on the board!")
                .AppendLine();

            foreach (var newChatMember in update.Message.NewChatMembers)
            {
                welcomeMessage.AppendLine($"@{newChatMember.Username ?? newChatMember.FirstName}");
            }

            await _telegramBot.SendTextMessageAsync(
                update.Message.Chat.Id,
                welcomeMessage.ToString(),
                parseMode: ParseMode.Html);

            return;
        }
    }

    public bool IsResponsibleForUpdate(Update update)
    {
        return update.Message?.Type == MessageType.ChatMembersAdded;
    }
}