using eShopOnTelegram.Domain.Services;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.Persistence.Context;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace eShopOnTelegram.Domain.IntegrationTests;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        var dbPath = Path.Join(path, "eShopOnTelegramTest.db");

        services
            .AddDbContext<EShopOnTelegramDbContext>(options => options.UseSqlite($"Data Source={dbPath}"))
            .AddSingleton<ILoggerFactory, LoggerFactory>()
            .AddTransient(typeof(ILogger<>), typeof(Logger<>))
            .AddTransient<IProductService, ProductService>()
            .AddTransient<IProductCategoryService, ProductCategoryService>();

        var serviceProvider = services.BuildServiceProvider();
        var dbContext = serviceProvider.GetRequiredService<EShopOnTelegramDbContext>();
        dbContext.Database.Migrate();
    }
}
