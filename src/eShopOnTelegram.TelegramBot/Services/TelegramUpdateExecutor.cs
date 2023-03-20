namespace eShopOnTelegram.TelegramBot.Services
{
    public class TelegramUpdateExecutor
    {
        private readonly TelegramCommandResolver _commandResolver;

        public TelegramUpdateExecutor(TelegramCommandResolver commandResolver)
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