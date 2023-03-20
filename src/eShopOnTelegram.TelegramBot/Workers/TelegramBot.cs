using eShopOnTelegram.TelegramBot.Services;

namespace eShopOnTelegram.TelegramBot.Workers
{
    public class TelegramBot : BackgroundService
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TelegramBot> _logger;

        public TelegramBot(
            ITelegramBotClient telegramBotClient,
            IServiceProvider serviceProvider,
            ILogger<TelegramBot> logger
        )
        {
            _telegramBotClient = telegramBotClient;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Telegram bot started...");
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
            };

            _telegramBotClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cancellationToken
            );
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Telegram bot stopped...");
            return base.StopAsync(cancellationToken);
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
            CancellationToken cancellationToken)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            var telegramUpdateExecutor = scope.ServiceProvider.GetRequiredService<TelegramUpdateExecutor>();

            await telegramUpdateExecutor.ExecuteAsync(update);
        }

        private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
            CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };


            _logger.LogError(exception, ErrorMessage);
            return Task.CompletedTask;
        }
    }
}