using eShopOnTelegram.TelegramBot.Commands;
using eShopOnTelegram.TelegramBot.Commands.Groups;
using eShopOnTelegram.TelegramBot.Commands.Interfaces;
using eShopOnTelegram.TelegramBot.Commands.Payment;
using eShopOnTelegram.TelegramBot.Commands.Payment.Invoice;
using eShopOnTelegram.TelegramBot.Services;
using eShopOnTelegram.TelegramBot.Services.Payment.Interfaces;
using eShopOnTelegram.TelegramBot.Services.Payment.TelegramButtonProviders;
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

            // Invoice generation commands
            services.AddScoped<ITelegramCommand, BankCardInvoiceSender>();
            services.AddScoped<ITelegramCommand, PlicioInvoiceSender>();

            // Payment telegram buttons generators
            services.AddSingleton<IPaymentTelegramButtonProvider, BankCardPaymentTelegramButtonProvider>();
            services.AddSingleton<IPaymentTelegramButtonProvider, PlicioPaymentTelegramButtonProvider>();

            // Telegram services
            services.AddScoped<CommandResolver>();
            services.AddScoped<UpdateExecutor>();
            services.AddScoped<PaymentMethodsSender>();
            services.AddSingleton<EmojiProvider>();

            return services;
        }
    }
}