using eShopOnTelegram.Shop.Worker.Services.Telegram;

using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;

namespace eShopOnTelegram.Shop.Worker;

public class TelegramBot : BackgroundService
{
    private readonly ITelegramBotClient _telegramBot;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TelegramBot> _logger;

    public TelegramBot(
        ITelegramBotClient telegramBot,
        IServiceProvider serviceProvider,
        ILogger<TelegramBot> logger
    )
    {
        _telegramBot = telegramBot;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Telegram bot started...");
        return base.StartAsync(cancellationToken);
    }

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
        };

        _telegramBot.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cancellationToken
        );

        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Telegram bot stopped...");
        return base.StopAsync(cancellationToken);
    }

    private Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        Task.Run(async () =>
        {
            try
            {
                using IServiceScope scope = _serviceProvider.CreateScope();
                var telegramUpdateResponseSender = scope.ServiceProvider.GetRequiredService<UpdateResponseSender>();

                await telegramUpdateResponseSender.SendResponse(update);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
            }
        }, cancellationToken);

        return Task.CompletedTask;
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