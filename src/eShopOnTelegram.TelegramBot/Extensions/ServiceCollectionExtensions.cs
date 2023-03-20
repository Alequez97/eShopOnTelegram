using eShopOnTelegram.TelegramBot.Commands;
using eShopOnTelegram.TelegramBot.Interfaces;
using eShopOnTelegram.TelegramBot.Services;

namespace eShopOnTelegram.TelegramBot.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTelegramCommandServices(this IServiceCollection services)
        {
            // Common commands
            services.AddScoped<UnknownCommand>();
            services.AddScoped<ITelegramCommand, StartCommand>();

            // Command services
            services.AddScoped<TelegramCommandResolver>();
            services.AddScoped<TelegramUpdateExecutor>();

            // Common telegram helper services
            services.AddSingleton<EmojiProvider>();

            return services;
        }
    }
}