using Azure.Core;
using Azure.Identity;

using eShopOnTelegram.Domain.Services;
using eShopOnTelegram.Persistence.Context;
using eShopOnTelegram.Persistence.Files.Interfaces;
using eShopOnTelegram.Persistence.Files.Stores;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.ApplicationInsights;

var builder = WebApplication.CreateBuilder(args);

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

// Add services to the container.
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();

builder.Services.AddScoped<IProductImagesStore, AzureBlobStorageProductImagesStore>();

builder.Services.AddDbContext<EShopOnTelegramDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("Sql")));

//builder.Services.AddCors(opt =>
//{
//    opt.AddPolicy("CorsPolicy", policy =>
//    {
//        policy
//            .AllowAnyOrigin()
//            .AllowAnyMethod()
//            .AllowAnyHeader();
//    });
//});

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

//app.UseCors("CorsPolicy");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
