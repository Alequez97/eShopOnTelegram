using System.Text;

using eShopOnTelegram.Notifications.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.BotOwnerData.Interfaces;

using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.Notifications;

public class TelegramNotificationSender : INotificationSender
{
    private readonly ITelegramBotClient _telegramBot;
    private readonly IBotOwnerDataStore _botOwnerDataStore;
    private readonly string _adminAppHostName;

    public TelegramNotificationSender(
        ITelegramBotClient telegramBot,
        IBotOwnerDataStore botOwnerDataStore,
        string adminAppHostName)
    {
        _telegramBot = telegramBot;
        _botOwnerDataStore = botOwnerDataStore;
        _adminAppHostName = adminAppHostName;
    }

    public async Task SendNotificationAsync(string title, string message, CancellationToken cancellationToken)
    {
        var chatIdAsString = await _botOwnerDataStore.GetBotOwnerTelegramGroupIdAsync(cancellationToken);
        var chatId = Convert.ToInt64(chatIdAsString);

        await _telegramBot.SendTextMessageAsync(
            chatId: chatId,
            text: message,
            parseMode: ParseMode.Html);
    }

    public async Task SendOrderReceivedNotificationAsync(string orderNumber, CancellationToken cancellationToken)
    {
        InlineKeyboardMarkup inlineKeyboard = null;
        var orderReceivedMessage = new StringBuilder("New order received!!!");

        if (!string.IsNullOrEmpty(_adminAppHostName))
        {
            var orderLink = $"{_adminAppHostName}/#/orders/{orderNumber}/show";

            orderReceivedMessage
                .AppendLine()
                .AppendLine("Click button for details");

            inlineKeyboard = new(new[]
            {
                InlineKeyboardButton.WithUrl(
                    text: "Show order details",
                    url: orderLink)
            });
        }

        var chatIdAsString = await _botOwnerDataStore.GetBotOwnerTelegramGroupIdAsync(cancellationToken);
        var chatId = Convert.ToInt64(chatIdAsString);

        await _telegramBot.SendTextMessageAsync(
            chatId: chatId,
            text: orderReceivedMessage.ToString(),
            parseMode: ParseMode.Html,
            replyMarkup: inlineKeyboard
        );
    }
}
