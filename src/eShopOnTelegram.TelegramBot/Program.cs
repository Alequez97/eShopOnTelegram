using Azure.Core;
using Azure.Identity;

using eShopOnTelegram.Domain.Services;
using eShopOnTelegram.ExternalServices.Extensions;
using eShopOnTelegram.ExternalServices.Services.Plisio;
using eShopOnTelegram.Persistence.Context;
using eShopOnTelegram.Persistence.Files.Interfaces;
using eShopOnTelegram.Persistence.Files.Stores;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Stores;
using eShopOnTelegram.RuntimeConfiguration.BotOwnerData.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.BotOwnerData.Stores;
using eShopOnTelegram.TelegramBot.Worker;
using eShopOnTelegram.TelegramBot.Worker.Appsettings;
using eShopOnTelegram.TelegramBot.Worker.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.ApplicationInsights;

var builder = WebApplication.CreateBuilder(args);

// Azure keyvault setup
var azureKeyVaultUriConfigValueSelector = "Azure:KeyVaultUri";
var azureKeyVaultUri = builder.Configuration[azureKeyVaultUriConfigValueSelector];

if (!string.IsNullOrWhiteSpace(azureKeyVaultUri))
{
    var tenantId = builder.Configuration["Azure:TenantId"];
    var clientId = builder.Configuration["Azure:ClientId"];
    var clientSecret = builder.Configuration["Azure:ClientSecret"];

    TokenCredential azureCredentials =
        string.IsNullOrWhiteSpace(tenantId)
     || string.IsNullOrWhiteSpace(clientId)
     || string.IsNullOrWhiteSpace(clientSecret) ? new DefaultAzureCredential() : new ClientSecretCredential(tenantId, clientId, clientSecret);

    builder.Configuration.AddAzureKeyVault(new Uri(azureKeyVaultUri), azureCredentials);
}

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
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();

// Telegram bot worker services
builder.Services.AddTelegramCommandServices();
builder.Services.AddSingleton<ITelegramBotClient>(_ =>
{
    var telegramAppsettings = builder.Configuration.GetSection<TelegramAppsettings>("Telegram");

    return new TelegramBotClient(telegramAppsettings.Token);
});
builder.Services.Configure<HostOptions>(hostOptions =>
{
    hostOptions.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
});
builder.Services.AddSingleton(builder.Configuration.GetSection<TelegramAppsettings>("Telegram"));
builder.Services.AddSingleton(builder.Configuration.GetSection<PaymentAppsettings>("Payment"));
builder.Services.AddHostedService<TelegramBot>();

builder.Services.AddControllersWithViews();

// External services
builder.Services
    .AddPolicyRegistry()
    .AddHttpRetryPolicy();

builder.Services.AddRefitServiceWithDefaultRetryPolicy<IPlicioClient>((_, httpClient) =>
{
    httpClient.BaseAddress = new Uri("https://plisio.net/api/v1");
});

// App insights logging
var appInsightsConnectionString = builder.Configuration["Azure:AppInsightsConnectionString"];
if (!string.IsNullOrWhiteSpace(appInsightsConnectionString))
{
    builder.Logging.AddApplicationInsights(
            configureTelemetryConfiguration: (config) =>
                config.ConnectionString = appInsightsConnectionString,
                configureApplicationInsightsLoggerOptions: (options) => { }
        );

    builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>(string.Empty, LogLevel.Information);
}

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

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
