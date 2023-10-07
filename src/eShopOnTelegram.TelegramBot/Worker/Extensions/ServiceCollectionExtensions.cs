using eShopOnTelegram.TelegramBot.Worker.Commands;
using eShopOnTelegram.TelegramBot.Worker.Commands.Groups;
using eShopOnTelegram.TelegramBot.Worker.Commands.Interfaces;
using eShopOnTelegram.TelegramBot.Worker.Commands.Orders;
using eShopOnTelegram.TelegramBot.Worker.Commands.Payment;
using eShopOnTelegram.TelegramBot.Worker.Commands.Payment.Invoice;
using eShopOnTelegram.TelegramBot.Worker.Services.Mappers;
using eShopOnTelegram.TelegramBot.Worker.Services.Payment.Interfaces;
using eShopOnTelegram.TelegramBot.Worker.Services.Payment.TelegramButtonProviders;
using eShopOnTelegram.TelegramBot.Worker.Services.Telegram;

namespace eShopOnTelegram.TelegramBot.Worker.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTelegramCommandServices(this IServiceCollection services)
        {
            // Common commands
            services.AddScoped<UnknownCommand>();
            services.AddScoped<ITelegramCommand, StartCommand>();

            // Telegram groups commands
            services.AddScoped<ITelegramCommand, MyChatMemberCommand>();
            services.AddScoped<ITelegramCommand, ChatMemberAddedCommand>();
            services.AddScoped<ITelegramCommand, ChatMemberLeftCommand>();

            // Order commands
            services.AddScoped<ITelegramCommand, CreateOrderCommand>();
            services.AddScoped<ITelegramCommand, ShowActiveOrderCommand>();

            // Payment commands
            services.AddScoped<ITelegramCommand, PreCheckoutQueryCommand>();
            services.AddScoped<ITelegramCommand, SuccessfulPaymentCommand>();

            // Invoice generation commands
            services.AddScoped<ITelegramCommand, BankCardInvoiceSender>();
            services.AddScoped<ITelegramCommand, PlisioInvoiceSender>();

            // Payment telegram buttons generators
            services.AddSingleton<IPaymentTelegramButtonProvider, BankCardPaymentTelegramButtonProvider>();
            services.AddSingleton<IPaymentTelegramButtonProvider, PlisioPaymentTelegramButtonProvider>();

            // Telegram services
            services.AddScoped<CommandResolver>();
            services.AddScoped<UpdateResponseSender>();
            services.AddScoped<PaymentProceedMessageSender>();

            // Mappers
            services.AddSingleton<EmojiKeyToUnicodeMapper>();
            services.AddSingleton<CurrencyCodeToSymbolMapper>();

            return services;
        }
    }
}