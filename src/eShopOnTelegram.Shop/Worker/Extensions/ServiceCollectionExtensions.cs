using eShopOnTelegram.Shop.Worker.Commands;
using eShopOnTelegram.Shop.Worker.Commands.Groups;
using eShopOnTelegram.Shop.Worker.Commands.Interfaces;
using eShopOnTelegram.Shop.Worker.Commands.Orders;
using eShopOnTelegram.Shop.Worker.Commands.Payment;
using eShopOnTelegram.Shop.Worker.Commands.Payment.Invoice;
using eShopOnTelegram.Shop.Worker.Services.Mappers;
using eShopOnTelegram.Shop.Worker.Services.Telegram;
using eShopOnTelegram.Shop.Worker.Services.Telegram.Buttons.Inline.Payment;
using eShopOnTelegram.Shop.Worker.Services.Telegram.Buttons.Inline.Payment.Interfaces;
using eShopOnTelegram.Shop.Worker.Services.Telegram.Buttons.Keyboard;
using eShopOnTelegram.Shop.Worker.Services.Telegram.Messages;

namespace eShopOnTelegram.Shop.Worker.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddTelegramCommandServices(this IServiceCollection services)
	{
		// Common commands
		services.AddScoped<UnknownCommand>();
		services.AddScoped<ITelegramCommand, StartCommand>();

		// Telegram groups commands
		services.AddScoped<ITelegramCommand, MyChatMemberCommand>();
		services.AddScoped<ITelegramCommand, GroupCreatedCommand>();
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
		services.AddScoped<IPaymentTelegramButtonProvider, BankCardPaymentTelegramButtonProvider>();
		services.AddScoped<IPaymentTelegramButtonProvider, PlisioPaymentTelegramButtonProvider>();
		services.AddScoped<IPaymentTelegramButtonProvider, PaymentThroughSellerTelegramButtonProvider>();

		// Open shop buttons layout provider
		services.AddScoped<OpenShopKeyboardButtonsLayoutProvider>();

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
