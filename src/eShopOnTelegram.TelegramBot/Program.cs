using Azure.Core;
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
using Microsoft.Extensions.Logging.ApplicationInsights;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) => {
        var configuration = configurationBuilder.Build();

        var azureKeyVaultUri = Environment.GetEnvironmentVariable("Azure__KeyVaultUri");
        if (!string.IsNullOrWhiteSpace(azureKeyVaultUri))
        {
            var tenantId = Environment.GetEnvironmentVariable("Azure__TenantId");
            var clientId = Environment.GetEnvironmentVariable("Azure__ClientId");
            var clientSecret = Environment.GetEnvironmentVariable("Azure__ClientSecret");

            TokenCredential azureCredentials =
                string.IsNullOrWhiteSpace(tenantId)
             || string.IsNullOrWhiteSpace(clientId)
             || string.IsNullOrWhiteSpace(clientSecret) ? new DefaultAzureCredential() : new ClientSecretCredential(tenantId, clientId, clientSecret);

            configurationBuilder.AddAzureKeyVault(new Uri(azureKeyVaultUri), azureCredentials);
        }
        else
        {
            azureKeyVaultUri = configuration["Azure:KeyVaultUri"];

            if (!string.IsNullOrWhiteSpace(azureKeyVaultUri))
            {
                var tenantId = configuration["Azure:TenantId"];
                var clientId = configuration["Azure:ClientId"];
                var clientSecret = configuration["Azure:ClientSecret"];

                TokenCredential azureCredentials =
                    string.IsNullOrWhiteSpace(tenantId)
                 || string.IsNullOrWhiteSpace(clientId)
                 || string.IsNullOrWhiteSpace(clientSecret) ? new DefaultAzureCredential() : new ClientSecretCredential(tenantId, clientId, clientSecret);

                configurationBuilder.AddAzureKeyVault(new Uri(azureKeyVaultUri), new DefaultAzureCredential());
            }
        }
    })
    .ConfigureLogging((hostBuilderContext, loggingBuilder) =>
    {
        var configuration = hostBuilderContext.Configuration;

        var appInsightsConnectionString = configuration["Azure:AppInsightsConnectionString"];
        if (!string.IsNullOrWhiteSpace(appInsightsConnectionString))
        {
            loggingBuilder.AddApplicationInsights(
                    configureTelemetryConfiguration: (config) =>
                        config.ConnectionString = appInsightsConnectionString,
                        configureApplicationInsightsLoggerOptions: (options) => { }
                );

            loggingBuilder.AddFilter<ApplicationInsightsLoggerProvider>(string.Empty, LogLevel.Information);
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