namespace eShopOnTelegram.TelegramBot.Services.Telegram;

public class UpdateResponseSender
{
    private readonly CommandResolver _commandResolver;

    public UpdateResponseSender(CommandResolver commandResolver)
    {
        _commandResolver = commandResolver;
    }

    public async Task SendResponse(Update update)
    {
        var command = await _commandResolver.ResolveAsync(update);

        await command.SendResponseAsync(update);
    }
}