using eShopOnTelegram.ApplicationContent.Interfaces;
using eShopOnTelegram.ApplicationContent.Keys;
using eShopOnTelegram.TelegramBot.Appsettings;
using eShopOnTelegram.TelegramBot.Constants;

namespace eShopOnTelegram.TelegramBot.Extensions;

public static class TelegramBotClientExtensions
{
    public static async Task SendDefaultErrorMessageAsync(this ITelegramBotClient telegramBot, long chatId, IApplicationContentStore applicationContentStore, CancellationToken cancellationToken)
    {
        await telegramBot.SendTextMessageAsync(
            chatId: chatId,
            text: await applicationContentStore.GetSingleValueAsync(ApplicationContentKey.TelegramBot.DefaultErrorMessage, CancellationToken.None),
            cancellationToken: cancellationToken);
    }
}
