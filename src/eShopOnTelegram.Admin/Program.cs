using System.Security.Claims;
using System.Text;

using Azure.Core;
using Azure.Identity;

using eShopOnTelegram.Domain.Services;
using eShopOnTelegram.Persistence.Context;
using eShopOnTelegram.Persistence.Entities;
using eShopOnTelegram.Persistence.Files.Interfaces;
using eShopOnTelegram.Persistence.Files.Stores;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Stores;
using eShopOnTelegram.Utils.Configuration;
using eShopOnTelegram.Utils.Extensions;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

ConfigureAzureKeyVault(builder);
var appSettings = ConfigureAppSettings(builder);
ConfigureServices(builder);
ConfigureDbContext(builder);
ConfigureApplicationInsights(builder, appSettings.AzureAppSettings);
ConfigureIdentity(builder);
ConfigureJWTAuthentication(builder, appSettings.JWTAuthOptions);

builder.Services.AddControllersWithViews();

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();

static void ConfigureAzureKeyVault(WebApplicationBuilder builder)
{
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
}

static AdminAppSettings ConfigureAppSettings(WebApplicationBuilder builder)
{
    var adminAppSettings = builder.Configuration.GetSection<AdminAppSettings>("AdminAppSettings");
    builder.Services.AddSingleton(adminAppSettings);
    return adminAppSettings;
}

static void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<IApplicationContentStore, AzureBlobStorageApplicationContentStore>();
    builder.Services.AddScoped<IApplicationDefaultContentStore, FileSystemDefaultContentStore>();

    // Add services to the container.
    builder.Services.AddScoped<IProductService, ProductService>();
    builder.Services.AddScoped<IProductAttributeService, ProductAttributeService>();
    builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();
    builder.Services.AddScoped<ICustomerService, CustomerService>();
    builder.Services.AddScoped<IOrderService, OrderService>();

    builder.Services.AddScoped<IProductImagesStore, AzureBlobStorageProductImagesStore>();
}

static void ConfigureApplicationInsights(WebApplicationBuilder builder, AdminAzureSettings azureAppSettings)
{
    //var appInsightsConnectionString = builder.Configuration["Azure:AppInsightsConnectionString"];
    if (!string.IsNullOrWhiteSpace(azureAppSettings.appInsightsConnectionString))
    {
        builder.Logging.AddApplicationInsights(
                configureTelemetryConfiguration: (config) =>
                    config.ConnectionString = azureAppSettings.appInsightsConnectionString,
                    configureApplicationInsightsLoggerOptions: (options) => { }
            );

        builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>(string.Empty, LogLevel.Information);
    }
}

static void ConfigureJWTAuthentication(WebApplicationBuilder builder, JWTAuthOptions authOptions)
{
    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = authOptions.Issuer,
            ValidAudience = authOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.Key)),
            ClockSkew = TimeSpan.Zero
        };
    });
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("RequireSuperadminClaim",
             policy => policy.RequireClaim(ClaimTypes.Role, new[] { "superadmin" }));
    });
}

static void ConfigureIdentity(WebApplicationBuilder builder)
{
    builder.Services.AddIdentity<User, IdentityRole<long>>()
        .AddEntityFrameworkStores<EShopOnTelegramDbContext>()
        .AddDefaultTokenProviders();
}

static void ConfigureDbContext(WebApplicationBuilder builder)
{
    builder.Services.AddDbContextFactory<EShopOnTelegramDbContext>(
        options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("Sql")));
}