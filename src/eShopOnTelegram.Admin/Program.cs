using System.Security.Claims;
using System.Text;

using Azure.Core;
using Azure.Identity;

using eShopOnTelegram.Admin.Constants;
using eShopOnTelegram.Domain.Services;
using eShopOnTelegram.Persistence.Context;
using eShopOnTelegram.Persistence.Entities.Users;
using eShopOnTelegram.Persistence.Files.Interfaces;
using eShopOnTelegram.Persistence.Files.Stores;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Stores;
using eShopOnTelegram.RuntimeConfiguration.Secrets;
using eShopOnTelegram.RuntimeConfiguration.Secrets.Constants;
using eShopOnTelegram.RuntimeConfiguration.Secrets.Interfaces;
using eShopOnTelegram.Translations.Interfaces;
using eShopOnTelegram.Translations.Services;
using eShopOnTelegram.Utils.AzureServiceManager;
using eShopOnTelegram.Utils.AzureServiceManager.Interfaces;
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
ConfigureApplicationInsights(builder, appSettings.AzureSettings);
ConfigureIdentity(builder);
ConfigureJWTAuthentication(builder, appSettings.JWTAuthSettings);

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

static AppSettings ConfigureAppSettings(WebApplicationBuilder builder)
{
	var appSettings = builder.Configuration.GetSection<AppSettings>("AppSettings");
	builder.Services.AddSingleton(appSettings);
	return appSettings;
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
	builder.Services.AddScoped<IPaymentService, PaymentService>();
	builder.Services.AddScoped<ITranslationsService, InMemoryTranslationsService>();

	builder.Services.AddScoped<IProductImagesStore, AzureBlobStorageProductImagesStore>();

	builder.Services.AddScoped<IKeyVaultClient, KeyVaultClient>();
	builder.Services.AddSingleton<ISecretsNameMapper, SecretsNameMapper>();
	builder.Services.AddSingleton<SecretsMappingConfig>();

	builder.Services.AddScoped<IAzureAppServiceManager, AzureAppServiceManager>();
}

static void ConfigureApplicationInsights(WebApplicationBuilder builder, AzureSettings azureAppSettings)
{
	if (!string.IsNullOrWhiteSpace(azureAppSettings.AppInsightsConnectionString))
	{
		builder.Logging.AddApplicationInsights(
				configureTelemetryConfiguration: (config) =>
					config.ConnectionString = azureAppSettings.AppInsightsConnectionString,
					configureApplicationInsightsLoggerOptions: (options) => { }
			);

		builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>(string.Empty, LogLevel.Information);
	}
}

static void ConfigureJWTAuthentication(WebApplicationBuilder builder, JWTAuthSettings authSettings)
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
			ValidIssuer = authSettings.Issuer,
			ValidAudience = authSettings.Audience,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.Key)),
			ClockSkew = TimeSpan.Zero
		};
	});
	builder.Services.AddAuthorization(options =>
	{
		options.AddPolicy(AuthPolicy.RequireSuperadminClaim,
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
