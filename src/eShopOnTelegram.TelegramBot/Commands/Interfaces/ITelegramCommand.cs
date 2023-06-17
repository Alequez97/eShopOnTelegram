namespace eShopOnTelegram.TelegramBot.Commands.Interfaces;

public interface ITelegramCommand
{
    Task SendResponseAsync(Update update);

    bool IsResponsibleForUpdate(Update update);
}