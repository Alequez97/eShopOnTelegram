using eShopOnTelegram.TelegramBot.Extensions;
using eShopOnTelegram.TelegramBot.Workers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        services.Configure<HostOptions>(hostOptions =>
        {
            hostOptions.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
        });

        services.AddSingleton<ITelegramBotClient>(_ =>
        {
            var telegramToken = configuration!["Telegram:Token"];

            return new TelegramBotClient(telegramToken);
        });

        services.AddTelegramCommandServices();

        services.AddHostedService<TelegramBot>();
    })
    .Build();

await host.RunAsync();
