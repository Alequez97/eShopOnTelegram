namespace eShopOnTelegram.Shop.Worker.Commands.Interfaces;

public interface ITelegramCommand
{
    Task SendResponseAsync(Update update);

    Task<bool> IsResponsibleForUpdateAsync(Update update);
}
