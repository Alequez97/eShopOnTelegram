namespace TelegramBot.Services.Telegram
{
    public class UpdateExecutor
    {
        private readonly CommandResolver _commandResolver;

        public UpdateExecutor(CommandResolver commandResolver)
        {
            _commandResolver = commandResolver;
        }

        public async Task ExecuteAsync(Update update)
        {
            var command = _commandResolver.Resolve(update);

            await command.SendResponseAsync(update);
        }
    }
}