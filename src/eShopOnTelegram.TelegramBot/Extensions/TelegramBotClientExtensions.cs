using eShopOnTelegram.ApplicationContent.Interfaces;
using eShopOnTelegram.ApplicationContent.Keys;

namespace eShopOnTelegram.TelegramBot.Extensions;

public static class TelegramBotClientExtensions
{
    public static async Task SendDefaultErrorMessageAsync(this ITelegramBotClient telegramBot, long chatId, IApplicationContentStore applicationContentStore, ILogger logger, CancellationToken cancellationToken)
    {
        try
        {
            var defaultErrorMessage = await applicationContentStore.GetSingleValueAsync(ApplicationContentKey.TelegramBot.DefaultErrorMessage, CancellationToken.None);

            await telegramBot.SendTextMessageAsync(
                chatId: chatId,
                text: defaultErrorMessage,
                cancellationToken: cancellationToken);
        }
        catch (Exception exception)
        {
            // This additional catch is required to send message in case when ApplicationContentStore contains error
            // In case when ApplicationContentStore is implemented incorrectry user will not receive message at all

            logger.LogError(exception, exception.Message);

            var fallbackErrorMessage = "Something went wrong. Try again later";

            await telegramBot.SendTextMessageAsync(
                chatId: chatId,
                text: fallbackErrorMessage,
                cancellationToken: cancellationToken);
        }

    }
}
