using Azure.Identity;

using eShopOnTelegram.Domain.Services;
using eShopOnTelegram.Persistence.Context;
using eShopOnTelegram.Persistence.Files.Interfaces;
using eShopOnTelegram.Persistence.Files.Stores;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Stores;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.ApplicationInsights;

var builder = WebApplication.CreateBuilder(args);

var azureKeyVaultUriConfigValueSelector = "Azure:KeyVaultUri";
var azureKeyVaultUri = Environment.GetEnvironmentVariable(azureKeyVaultUriConfigValueSelector);
if (!string.IsNullOrWhiteSpace(azureKeyVaultUri))
{
    builder.Configuration.AddAzureKeyVault(new Uri(azureKeyVaultUri), new ClientSecretCredential("bf6ef353-a632-470c-8235-e122f2ebf99d", "aae3d44a-bdef-42ea-82cb-53f729a8886a", "X1K8Q~gjcjB0-pCC89bbBybuMa.af.4MlHHFuaBS"));
}
else
{
    var configurationValue = builder.Configuration[azureKeyVaultUriConfigValueSelector];
    if (!string.IsNullOrWhiteSpace(configurationValue))
    {
        azureKeyVaultUri = configurationValue;
        builder.Configuration.AddAzureKeyVault(new Uri(azureKeyVaultUri), new ClientSecretCredential("bf6ef353-a632-470c-8235-e122f2ebf99d", "aae3d44a-bdef-42ea-82cb-53f729a8886a", "X1K8Q~gjcjB0-pCC89bbBybuMa.af.4MlHHFuaBS"));
    }
}

builder.Services.AddScoped<IApplicationContentStore, AzureBlobStorageApplicationContentStore>();
builder.Services.AddScoped<IApplicationDefaultContentStore, FileSystemDefaultContentStore>();

// Add services to the container.
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddScoped<IProductImagesStore, AzureBlobStorageProductImagesStore>();

builder.Services.AddDbContext<EShopOnTelegramDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("Sql")));

builder.Services.AddControllersWithViews();

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

app.MapFallbackToFile("index.html");

app.Run();
