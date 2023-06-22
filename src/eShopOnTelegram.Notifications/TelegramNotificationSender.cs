using eShopOnTelegram.Notifications.Interfaces;

using Telegram.Bot;

namespace eShopOnTelegram.Notifications;

public class TelegramNotificationSender : INotificationSender
{
    private readonly ITelegramBotClient _telegramBot;
    private readonly long _chatId;

    public TelegramNotificationSender(ITelegramBotClient telegramBot, long chatId)
    {
        _telegramBot = telegramBot;
        _chatId = chatId;
    }

    public void SendNotificationAsync(string title, string message)
    {
        _telegramBot.SendTextMessageAsync(_chatId, string.Join("\n", title, message));
    }
}
