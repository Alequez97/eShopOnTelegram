namespace eShopOnTelegram.TelegramBot.Services.Telegram;

public class UpdateExecutor
{
    private readonly CommandResolver _commandResolver;

    public UpdateExecutor(CommandResolver commandResolver)
    {
        _commandResolver = commandResolver;
    }

    public async Task ExecuteAsync(Update update)
    {
        var command = await _commandResolver.ResolveAsync(update);

        await command.SendResponseAsync(update);
    }
}