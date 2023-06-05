using eShopOnTelegram.TelegramBot.Commands;
using eShopOnTelegram.TelegramBot.Commands.Groups;
using eShopOnTelegram.TelegramBot.Commands.Payment;
using eShopOnTelegram.TelegramBot.Services;

using TelegramBot.Commands.Interfaces;

namespace eShopOnTelegram.TelegramBot.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTelegramCommandServices(this IServiceCollection services)
        {
            //Common commands
            services.AddScoped<UnknownCommand>();
            services.AddScoped<ITelegramCommand, StartCommand>();
            services.AddScoped<ITelegramCommand, WebAppCommand>();

            //Telegram groups commands
            services.AddScoped<ITelegramCommand, MyChatMemberCommand>();
            services.AddScoped<ITelegramCommand, ChatMemberAddedCommand>();
            services.AddScoped<ITelegramCommand, ChatMemberLeftCommand>();

            //Payment commands
            services.AddScoped<ITelegramCommand, PreCheckoutQueryCommand>();
            services.AddScoped<ITelegramCommand, SuccessfulPaymentCommand>();

            //Command services
            services.AddScoped<TelegramCommandResolver>();
            services.AddScoped<TelegramUpdateExecutor>();

            // Common telegram helper services
            services.AddSingleton<EmojiProvider>();

            return services;
        }
    }
}