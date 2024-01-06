using Ardalis.GuardClauses;

using eShopOnTelegram.Domain.Requests.Orders;
using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Notifications.Interfaces;
using eShopOnTelegram.Persistence.Entities.Orders;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Keys;
using eShopOnTelegram.Shop.Worker.Commands.Interfaces;
using eShopOnTelegram.Shop.Worker.Extensions;
using eShopOnTelegram.Utils.Extensions;

namespace eShopOnTelegram.Shop.Worker.Commands.Payment;

public class SuccessfulPaymentCommand : ITelegramCommand
{
	private readonly ITelegramBotClient _telegramBot;
	private readonly IOrderService _orderService;
	private readonly IPaymentService _paymentService;
	private readonly IApplicationContentStore _applicationContentStore;
	private readonly IEnumerable<INotificationSender> _notificationSenders;
	private readonly ILogger<SuccessfulPaymentCommand> _logger;

	public SuccessfulPaymentCommand(
		ITelegramBotClient telegramBot,
		IOrderService orderService,
		IPaymentService paymentService,
		IApplicationContentStore applicationContentStore,
		IEnumerable<INotificationSender> notificationSenders,
		ILogger<SuccessfulPaymentCommand> logger)
	{
		_telegramBot = telegramBot;
		_orderService = orderService;
		_paymentService = paymentService;
		_applicationContentStore = applicationContentStore;
		_notificationSenders = notificationSenders;
		_logger = logger;
	}

	public async Task SendResponseAsync(Update update)
	{
		Guard.Against.Null(update.Message);
		Guard.Against.Null(update.Message.SuccessfulPayment);

		var chatId = update.Message.Chat.Id;

		try
		{
			var orderNumber = update.Message.SuccessfulPayment.InvoicePayload;

			var response = await _paymentService.ConfirmOrderPayment(orderNumber, PaymentMethod.Card);

			if (response.Status != ResponseStatus.Success)
			{
				throw new Exception("Failed to confirm order payment in SuccessfulPayment TG Command.");
			}

			await _telegramBot.SendTextMessageAsync(
				chatId,
				await _applicationContentStore.GetValueAsync(ApplicationContentKey.Payment.SuccessfullPayment, CancellationToken.None)
			);

			foreach (var notificationSender in _notificationSenders)
			{
				await notificationSender.SendOrderReceivedNotificationAsync(orderNumber, CancellationToken.None);
			}

			var deliveryAddress = new UpdateDeliveryAddressRequest()
			{
				CountryIso2Code = update.Message.SuccessfulPayment.OrderInfo.ShippingAddress.CountryCode.ToNullIfEmptyOrWhiteSpace(),
				City = update.Message.SuccessfulPayment.OrderInfo.ShippingAddress.City.ToNullIfEmptyOrWhiteSpace(),
				StreetLine1 = update.Message.SuccessfulPayment.OrderInfo.ShippingAddress.StreetLine1.ToNullIfEmptyOrWhiteSpace(),
				StreetLine2 = update.Message.SuccessfulPayment.OrderInfo.ShippingAddress.StreetLine2.ToNullIfEmptyOrWhiteSpace(),
				PostCode = update.Message.SuccessfulPayment.OrderInfo.ShippingAddress.PostCode.ToNullIfEmptyOrWhiteSpace()
			};
			var updateAddressResponse = await _orderService.UpdateDeliveryAddressAsync(orderNumber, deliveryAddress, CancellationToken.None);

			if (updateAddressResponse.Status != ResponseStatus.Success)
			{
				await _telegramBot.SendTextMessageAsync(
					chatId,
					await _applicationContentStore.GetValueAsync(ApplicationContentKey.Order.UnableToGetShippingAddress, CancellationToken.None)
				);
			}
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, exception.Message);
			await _telegramBot.SendDefaultErrorMessageAsync(chatId, _applicationContentStore, _logger, CancellationToken.None);
		}
	}

	public Task<bool> IsResponsibleForUpdateAsync(Update update)
	{
		return Task.FromResult(update.Message?.Type == MessageType.SuccessfulPayment);
	}
}
