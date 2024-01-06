using eShopOnTelegram.Domain.Services;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.Persistence.Context;
using eShopOnTelegram.Persistence.Files.Interfaces;
using eShopOnTelegram.Persistence.Files.Stores;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace eShopOnTelegram.Domain.IntegrationTests;

public class Startup
{
	public void ConfigureServices(IServiceCollection services)
	{
		var config = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json")
			.AddUserSecrets(typeof(Startup).Assembly)
			.AddEnvironmentVariables()
			.Build();

		services
			.AddSingleton<IConfiguration>(config)
			.AddDbContextFactory<EShopOnTelegramDbContext>(options => options.UseSqlServer(config.GetConnectionString("Sql")))
			.AddSingleton<ILoggerFactory, LoggerFactory>()
			.AddTransient(typeof(ILogger<>), typeof(Logger<>))
			.AddTransient<IProductService, ProductService>()
			.AddTransient<IProductAttributeService, ProductAttributeService>()
			.AddTransient<IProductImagesStore, AzureBlobStorageProductImagesStore>()
			.AddTransient<IOrderService, OrderService>()
			.AddTransient<ICustomerService, CustomerService>()
			.AddTransient<IProductCategoryService, ProductCategoryService>();

		var serviceProvider = services.BuildServiceProvider();
		var dbContext = serviceProvider.GetRequiredService<EShopOnTelegramDbContext>();
		dbContext.Database.Migrate();
	}
}
