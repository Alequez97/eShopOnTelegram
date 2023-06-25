using eshopOnTelegram.TelegramBot.Appsettings;

using eShopOnTelegram.ApplicationContent.Interfaces;
using eShopOnTelegram.ApplicationContent.Stores;
using eShopOnTelegram.Domain.Services;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.ExternalServices.Extensions;
using eShopOnTelegram.ExternalServices.Services.Plisio;
using eShopOnTelegram.Persistence.Context;
using eShopOnTelegram.TelegramBot.Appsettings;
using eShopOnTelegram.TelegramBot.Extensions;
using eShopOnTelegram.TelegramBot.Workers;

using Microsoft.EntityFrameworkCore;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        services.Configure<HostOptions>(hostOptions =>
        {
            hostOptions.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
        });

        services.AddDbContext<EShopOnTelegramDbContext>(
            options => options.UseSqlServer(configuration.GetConnectionString("Sql")));

        services.AddSingleton<ITelegramBotClient>(_ =>
        {
            var telegramAppsettings = configuration.GetSection<TelegramAppsettings>("Telegram");

            return new TelegramBotClient(telegramAppsettings.Token);
        });

        services.AddScoped<IApplicationContentStore, AzureBlobStorageApplicationContentStore>();
        services.AddScoped<IApplicationDefaultContentStore, FileSystemDefaultContentStore>();

        services.AddSingleton(configuration.GetSection<TelegramAppsettings>("Telegram"));
        services.AddSingleton(configuration.GetSection<PaymentAppsettings>("Payment"));

        services.AddTelegramCommandServices();

        services.AddHostedService<TelegramBot>();

        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IOrderService, OrderService>();

        services
            .AddPolicyRegistry()
            .AddHttpRetryPolicy();

        services.AddRefitServiceWithDefaultRetryPolicy<IPlicioClient>((_, httpClient) =>
        {
            httpClient.BaseAddress = new Uri("https://plisio.net/api/v1");
        });
    })
    .Build();

await host.RunAsync();