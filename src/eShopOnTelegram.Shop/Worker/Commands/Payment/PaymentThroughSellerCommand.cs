using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Keys;
using eShopOnTelegram.Shop.Worker.Commands.Interfaces;
using eShopOnTelegram.Shop.Worker.Constants;
using eShopOnTelegram.Shop.Worker.Extensions;

namespace eShopOnTelegram.Shop.Worker.Commands.Payment;

public class PaymentThroughSellerCommand : ITelegramCommand
{
	private readonly IOrderService _orderService;
	private readonly IPaymentService _paymentService;
	private readonly ITelegramBotClient _telegramBot;
	private readonly IApplicationContentStore _applicationContentStore;
	private readonly ILogger<PaymentThroughSellerCommand> _logger;

	public PaymentThroughSellerCommand(
		IOrderService orderService,
		IPaymentService paymentService,
		ITelegramBotClient telegramBot,
		IApplicationContentStore applicationContentStore,
		ILogger<PaymentThroughSellerCommand> logger)
	{
		_orderService = orderService;
		_paymentService = paymentService;
		_telegramBot = telegramBot;
		_applicationContentStore = applicationContentStore;
		_logger = logger;
	}

	public async Task SendResponseAsync(Update update)
	{
		var chatId = update.CallbackQuery.From.Id;

		try
		{
			var getOrdersResponse = await _orderService.GetUnpaidOrderByTelegramIdAsync(chatId, CancellationToken.None);

			if (getOrdersResponse.Status != ResponseStatus.Success)
			{
				await _telegramBot.SendTextMessageAsync(chatId, await _applicationContentStore.GetValueAsync(ApplicationContentKey.Order.InvoiceGenerationFailedErrorMessage, CancellationToken.None));
				return;
			}
			if (getOrdersResponse.Data.PaymentMethodSelected)
			{
				await _telegramBot.SendTextMessageAsync(chatId, await _applicationContentStore.GetValueAsync(ApplicationContentKey.Order.PaymentMethodAlreadySelected, CancellationToken.None));
				return;
			}

			var response = await _paymentService.UpdateOrderPaymentMethod(getOrdersResponse.Data.OrderNumber, Persistence.Entities.Orders.PaymentMethod.PaymentThroughSeller);
			if (response.Status != ResponseStatus.Success)
			{
				throw new Exception("Failed to update order payment method in PaymentThroughSeller TG Command.");
			}

			await _telegramBot.SendTextMessageAsync(
				chatId: chatId,
				text: await _applicationContentStore.GetValueAsync(ApplicationContentKey.Payment.PaymentThroughSeller, CancellationToken.None),
				cancellationToken: CancellationToken.None);
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, exception.Message);
			await _telegramBot.SendDefaultErrorMessageAsync(chatId, _applicationContentStore, _logger, CancellationToken.None);
		}
	}

	public Task<bool> IsResponsibleForUpdateAsync(Update update)
	{
		return Task.FromResult(update.Type == UpdateType.CallbackQuery && update.CallbackQuery.Data.Equals(PaymentMethodConstants.PaymentThroughSeller));
	}
}
