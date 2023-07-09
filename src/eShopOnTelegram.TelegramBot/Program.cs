
using Azure.Identity;

using eshopOnTelegram.TelegramBot.Appsettings;

using eShopOnTelegram.Domain.Services;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.ExternalServices.Extensions;
using eShopOnTelegram.ExternalServices.Services.Plisio;
using eShopOnTelegram.Persistence.Context;
using eShopOnTelegram.Persistence.Files.Interfaces;
using eShopOnTelegram.Persistence.Files.Stores;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Stores;
using eShopOnTelegram.RuntimeConfiguration.BotOwnerData.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.BotOwnerData.Stores;
using eShopOnTelegram.TelegramBot.Appsettings;
using eShopOnTelegram.TelegramBot.Extensions;
using eShopOnTelegram.TelegramBot.Workers;

using Microsoft.EntityFrameworkCore;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) => {
        var configuration = configurationBuilder.Build();
        var azureKeyVaultUriConfigValueSelector = "Azure:KeyVaultUri";
        string azureKeyVaultUri;

        azureKeyVaultUri = Environment.GetEnvironmentVariable(azureKeyVaultUriConfigValueSelector);
        if (!string.IsNullOrWhiteSpace(azureKeyVaultUri))
        {
            configurationBuilder.AddAzureKeyVault(new Uri(azureKeyVaultUri), new DefaultAzureCredential());
            return;
        }

        azureKeyVaultUri = configuration[azureKeyVaultUriConfigValueSelector];
        if (!string.IsNullOrWhiteSpace(azureKeyVaultUri))
        {
            configurationBuilder.AddAzureKeyVault(new Uri(azureKeyVaultUri), new DefaultAzureCredential());
            return;
        }
    })
    .ConfigureServices(services =>
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        services.Configure<HostOptions>(hostOptions =>
        {
            hostOptions.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
        });

        var sqlConnectionString = configuration.GetConnectionString("Sql");
        services.AddDbContext<EShopOnTelegramDbContext>(
            options => options.UseSqlServer(sqlConnectionString));

        services.AddScoped<IProductImagesStore, AzureBlobStorageProductImagesStore>();

        services.AddSingleton<ITelegramBotClient>(_ =>
        {
            var telegramAppsettings = configuration.GetSection<TelegramAppsettings>("Telegram");

            return new TelegramBotClient(telegramAppsettings.Token);
        });

        services.AddScoped<IApplicationContentStore, AzureBlobStorageApplicationContentStore>();
        services.AddScoped<IApplicationDefaultContentStore, FileSystemDefaultContentStore>();
        
        services.AddScoped<IBotOwnerDataStore, AzureBlobStorageBotOwnerDataStore>();

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