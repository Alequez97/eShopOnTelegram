using eShopOnTelegram.Domain.Services;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.Persistence.Context;

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
            .AddDbContext<EShopOnTelegramDbContext>(options => options.UseSqlServer(config.GetConnectionString("Sql")))
            .AddSingleton<ILoggerFactory, LoggerFactory>()
            .AddTransient(typeof(ILogger<>), typeof(Logger<>))
            .AddTransient<IProductService, ProductService>()
            .AddTransient<IOrderService, OrderService>()
            .AddTransient<ICustomerService, CustomerService>()
            .AddTransient<IProductCategoryService, ProductCategoryService>();

        var serviceProvider = services.BuildServiceProvider();
        var dbContext = serviceProvider.GetRequiredService<EShopOnTelegramDbContext>();
        dbContext.Database.Migrate();
    }
}
