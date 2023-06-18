using eShopOnTelegram.TelegramBot.Commands.Interfaces;

namespace eShopOnTelegram.TelegramBot.Commands.Payment;

public class PreCheckoutQueryCommand : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBotClient;

    public PreCheckoutQueryCommand(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }

    public async Task SendResponseAsync(Update update)
    {
        var preCheckoutQuery = update.PreCheckoutQuery;

        // Here data can be validated and desicion can be made if order can processed
        // In our case this should be done in WebAppData command since it sends invoice

        await _telegramBotClient.AnswerPreCheckoutQueryAsync(preCheckoutQuery.Id);
    }

    public bool IsResponsibleForUpdate(Update update)
    {
        return update.PreCheckoutQuery != null;
    }
}