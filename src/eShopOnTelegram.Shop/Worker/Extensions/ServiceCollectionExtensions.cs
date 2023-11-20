using eShopOnTelegram.Shop.Worker.Commands;
using eShopOnTelegram.Shop.Worker.Commands.Groups;
using eShopOnTelegram.Shop.Worker.Commands.Interfaces;
using eShopOnTelegram.Shop.Worker.Commands.Orders;
using eShopOnTelegram.Shop.Worker.Commands.Payment;
using eShopOnTelegram.Shop.Worker.Commands.Payment.Invoice;
using eShopOnTelegram.Shop.Worker.Services.Mappers;
using eShopOnTelegram.Shop.Worker.Services.Payment.Interfaces;
using eShopOnTelegram.Shop.Worker.Services.Payment.TelegramButtonProviders;
using eShopOnTelegram.Shop.Worker.Services.Telegram;

namespace eShopOnTelegram.Shop.Worker.Extensions
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
            services.AddScoped<ITelegramCommand, PaymentThroughSellerCommand>();

            // Payment telegram buttons generators
            services.AddSingleton<IPaymentTelegramButtonProvider, BankCardPaymentTelegramButtonProvider>();
            services.AddSingleton<IPaymentTelegramButtonProvider, PlisioPaymentTelegramButtonProvider>();
            services.AddSingleton<IPaymentTelegramButtonProvider, PaymentThroughSellerTelegramButtonProvider>();

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