using eShopOnTelegram.TelegramBot.Commands;
using eShopOnTelegram.TelegramBot.Commands.Groups;
using eShopOnTelegram.TelegramBot.Commands.Interfaces;
using eShopOnTelegram.TelegramBot.Commands.Payment;
using eShopOnTelegram.TelegramBot.Services;
using eShopOnTelegram.TelegramBot.Services.Telegram;

namespace eShopOnTelegram.TelegramBot.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTelegramCommandServices(this IServiceCollection services)
        {
            // Common commands
            services.AddScoped<UnknownCommand>();
            services.AddScoped<ITelegramCommand, StartCommand>();
            services.AddScoped<ITelegramCommand, WebAppCommand>();

            // Telegram groups commands
            services.AddScoped<ITelegramCommand, MyChatMemberCommand>();
            services.AddScoped<ITelegramCommand, ChatMemberAddedCommand>();
            services.AddScoped<ITelegramCommand, ChatMemberLeftCommand>();

            // Payment commands
            services.AddScoped<ITelegramCommand, PreCheckoutQueryCommand>();
            services.AddScoped<ITelegramCommand, SuccessfulPaymentCommand>();

            // Telegram services
            services.AddScoped<CommandResolver>();
            services.AddScoped<UpdateExecutor>();
            services.AddScoped<InvoiceSender>();
            services.AddSingleton<EmojiProvider>();

            return services;
        }
    }
}