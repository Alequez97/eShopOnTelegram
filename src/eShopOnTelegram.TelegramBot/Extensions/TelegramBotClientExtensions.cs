using eShopOnTelegram.TelegramBot.Appsettings;
using eShopOnTelegram.TelegramBot.Constants;

namespace eShopOnTelegram.TelegramBot.Extensions;

public static class TelegramBotClientExtensions
{
    public static async Task SendCommonErrorMessageAsync(this ITelegramBotClient telegramBot, long chatId, BotContentAppsettings botContentAppsettings, CancellationToken cancellationToken)
    {
        await telegramBot.SendTextMessageAsync(
            chatId: chatId,
            text: botContentAppsettings.Common.DefaultErrorMessage ?? BotContentDefaultConstants.Common.DefaultErrorMessage,
            cancellationToken: cancellationToken);
    }
}
