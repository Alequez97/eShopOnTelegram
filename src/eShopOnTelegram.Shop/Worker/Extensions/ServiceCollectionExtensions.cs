using eShopOnTelegram.Shop.Worker.Commands;
using eShopOnTelegram.Shop.Worker.Commands.Groups;
using eShopOnTelegram.Shop.Worker.Commands.Interfaces;
using eShopOnTelegram.Shop.Worker.Commands.Orders;
using eShopOnTelegram.Shop.Worker.Commands.Payment;
using eShopOnTelegram.Shop.Worker.Commands.Payment.Invoice;
using eShopOnTelegram.Shop.Worker.Commands.Products;
using eShopOnTelegram.Shop.Worker.Commands.Shop;
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
		services.AddScoped<ITelegramCommand, CreateOrderWebAppCommand>();
		services.AddScoped<ITelegramCommand, ShowActiveOrderCommand>();

		// Telegram bank card payments commands
		services.AddScoped<ITelegramCommand, PreCheckoutQueryCommand>();
		services.AddScoped<ITelegramCommand, SuccessfulPaymentCommand>();

		// Inline buttons shop layout commands
		services.AddScoped<ITelegramCommand, OpenShopCommand>();
		services.AddScoped<ITelegramCommand, ShowProductsCommand>();
		services.AddScoped<ITelegramCommand, ShowProductCategoriesCommand>();
		services.AddScoped<ITelegramCommand, ShowQuantitySelectorCommand>();
		services.AddScoped<ITelegramCommand, ShowAvailablePaymentMethodsCommand>();

		// Invoice generation commands
		services.AddScoped<ITelegramCommand, BankCardInvoiceCommand>();
		services.AddScoped<ITelegramCommand, PlisioInvoiceCommand>();
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
		services.AddScoped<ChoosePaymentMethodSender>();
		services.AddScoped<OrderSummarySender>();

		// Mappers
		services.AddSingleton<EmojiKeyToUnicodeMapper>();
		services.AddSingleton<CurrencyCodeToSymbolMapper>();

		return services;
	}

	public static void ValidateAllTelegramCommandsAreRegistered(this IServiceProvider serviceProvider)
	{
		var commandTypes = typeof(ITelegramCommand).Assembly.GetTypes()
			.Where(type => type.IsClass && !type.IsAbstract && typeof(ITelegramCommand).IsAssignableFrom(type));

		var registeredCommands = serviceProvider.GetServices<ITelegramCommand>().Select(c => c.GetType());

		foreach (var commandType in commandTypes)
		{
			var service = serviceProvider.GetService(commandType);

			if (service == null)
			{
				throw new InvalidOperationException($"Implementation for {commandType.Name} is not registered.");
			}
		}
	}
}
