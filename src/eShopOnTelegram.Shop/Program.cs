using Azure.Core;
using Azure.Identity;

using eShopOnTelegram.Domain.Services;
using eShopOnTelegram.ExternalServices.Extensions;
using eShopOnTelegram.ExternalServices.Interfaces;
using eShopOnTelegram.ExternalServices.Services.Plisio;
using eShopOnTelegram.ExternalServices.Services.Plisio.Requests;
using eShopOnTelegram.ExternalServices.Services.Plisio.Validators;
using eShopOnTelegram.Notifications;
using eShopOnTelegram.Notifications.Interfaces;
using eShopOnTelegram.Persistence.Context;
using eShopOnTelegram.Persistence.Files.Interfaces;
using eShopOnTelegram.Persistence.Files.Stores;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Stores;
using eShopOnTelegram.RuntimeConfiguration.BotOwnerData.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.BotOwnerData.Stores;
using eShopOnTelegram.TelegramBot.Api.Middlewares;
using eShopOnTelegram.TelegramBot.Worker;
using eShopOnTelegram.TelegramBot.Worker.Extensions;
using eShopOnTelegram.Utils.Configuration;
using eShopOnTelegram.Utils.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.ApplicationInsights;

var builder = WebApplication.CreateBuilder(args);

ConfigureAzureKeyVault(builder);

var appSettings = ConfigureAppSettings(builder);

ConfigureServices(builder, appSettings.AzureSettings);
ConfigureTelegramBotWorkerServices(builder, appSettings.TelegramBotSettings);
ConfigureHostOptions(builder);

// External services
builder.Services
    .AddPolicyRegistry()
    .AddHttpRetryPolicy();

ConfigurePlisio(builder, appSettings.PaymentSettings);
ConfigureNotificationSenders(builder);

// Controllers and views
builder.Services.AddControllersWithViews();

ConfigureApplicationInsights(builder, appSettings.AzureSettings);

// App building
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EShopOnTelegramDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAuthorizeTelegram();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();

static AppSettings ConfigureAppSettings(WebApplicationBuilder builder)
{
    var appSettings = builder.Configuration.GetSection<AppSettings>("AppSettings");

    builder.Services.AddSingleton(appSettings);
    return appSettings;
}

static void ConfigureAzureKeyVault(WebApplicationBuilder builder)
{
    var azureAppSettings = builder.Configuration.GetSection<AppSettings>("AppSettings").AzureSettings;

    if (!string.IsNullOrWhiteSpace(azureAppSettings.KeyVaultUri))
    {
        TokenCredential azureCredentials =
            string.IsNullOrWhiteSpace(azureAppSettings.TenantId)
         || string.IsNullOrWhiteSpace(azureAppSettings.ClientId)
         || string.IsNullOrWhiteSpace(azureAppSettings.ClientSecret)
         ?
         new DefaultAzureCredential()
         :
         new ClientSecretCredential(azureAppSettings.TenantId, azureAppSettings.ClientId, azureAppSettings.ClientSecret);

        builder.Configuration.AddAzureKeyVault(new Uri(azureAppSettings.KeyVaultUri), azureCredentials);
    }
}

static void ConfigureServices(WebApplicationBuilder builder, AzureSettings azureSettings)
{
    // Persistence layer services
    builder.Services.AddScoped<IProductImagesStore, AzureBlobStorageProductImagesStore>();
    builder.Services.AddScoped<IApplicationDefaultContentStore, FileSystemDefaultContentStore>();
    builder.Services.AddScoped<IApplicationContentStore, AzureBlobStorageApplicationContentStore>();
    builder.Services.AddScoped<IBotOwnerDataStore, AzureBlobStorageBotOwnerDataStore>();

    builder.Services.AddDbContextFactory<EShopOnTelegramDbContext>(
        options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("Sql")));

    // Domain layer services
    builder.Services.AddScoped<IOrderService, OrderService>();
    builder.Services.AddScoped<ICustomerService, CustomerService>();
    builder.Services.AddScoped<IProductService, ProductService>();
    builder.Services.AddScoped<IProductAttributeService, ProductAttributeService>();
    builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();
}

static void ConfigureTelegramBotWorkerServices(WebApplicationBuilder builder, TelegramBotSettings telegramBotSettings)
{
    // Telegram bot worker services
    builder.Services.AddTelegramCommandServices();
    builder.Services.AddSingleton<ITelegramBotClient>(_ =>
    {
        return new TelegramBotClient(telegramBotSettings.Token);
    });
    builder.Services.AddHostedService<TelegramBot>();
}

static void ConfigureHostOptions(WebApplicationBuilder builder)
{
    builder.Services.Configure<HostOptions>(hostOptions =>
    {
        hostOptions.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
    });
}

static void ConfigureApplicationInsights(WebApplicationBuilder builder, AzureSettings azureSettings)
{
    // App insights logging
    if (!string.IsNullOrWhiteSpace(azureSettings.AppInsightsConnectionString))
    {
        builder.Logging.AddApplicationInsights(
                configureTelemetryConfiguration: (config) =>
                    config.ConnectionString = azureSettings.AppInsightsConnectionString,
                    configureApplicationInsightsLoggerOptions: (options) => { }
            );

        builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>(string.Empty, LogLevel.Information);
    }
}

static void ConfigureNotificationSenders(WebApplicationBuilder builder)
{
    // Notification senders
    builder.Services.AddScoped<INotificationSender>((provider) =>
    {
        var telegramBot = provider.GetRequiredService<ITelegramBotClient>();
        var botOwnerDataStore = provider.GetRequiredService<IBotOwnerDataStore>();
        var adminAppHostName = builder.Configuration["AdminAppHostName"];

        return new TelegramNotificationSender(telegramBot, botOwnerDataStore, adminAppHostName);
    });
}

static void ConfigurePlisio(WebApplicationBuilder builder, PaymentSettings paymentSettings)
{
    builder.Services.AddRefitServiceWithDefaultRetryPolicy<IPlisioClient>((_, httpClient) =>
    {
        httpClient.BaseAddress = new Uri("https://plisio.net/api/v1");
    });

    // External services webhook validators
    builder.Services.AddScoped<IWebhookRequestValidator<PlisioPaymentReceivedWebhookRequest>>(_ =>
    {
        return new PlisioPaymentReceivedWebhookRequestValidator(paymentSettings.Plisio.ApiToken);
    });
}