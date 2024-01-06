using Ardalis.GuardClauses;

using eShopOnTelegram.Domain.Requests.Orders;
using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Keys;
using eShopOnTelegram.Shop.Worker.Commands.Interfaces;
using eShopOnTelegram.Shop.Worker.Extensions;
using eShopOnTelegram.Shop.Worker.Services.Telegram;

using Newtonsoft.Json;

namespace eShopOnTelegram.Shop.Worker.Commands.Orders;

/// <summary>
/// <para>Currently this command is used to create new order.
/// This command is executed when sendData(data: string) function is executed in web app</para>
/// <i>Note: sendData() function is available only when web app is opened with telegram keyboard button</i>
/// </summary>
public class CreateOrderCommand : ITelegramCommand
{
	private readonly ILogger<CreateOrderCommand> _logger;
	private readonly ITelegramBotClient _telegramBot;
	private readonly IOrderService _orderService;
	private readonly PaymentProceedMessageSender _paymentMethodsSender;
	private readonly IApplicationContentStore _applicationContentStore;

	public CreateOrderCommand(
		ILogger<CreateOrderCommand> logger,
		ITelegramBotClient telegramBot,
		IOrderService orderService,
		PaymentProceedMessageSender paymentMethodsSender,
		IApplicationContentStore applicationContentStore)
	{
		_logger = logger;
		_telegramBot = telegramBot;
		_orderService = orderService;
		_paymentMethodsSender = paymentMethodsSender;
		_applicationContentStore = applicationContentStore;
	}

	public async Task SendResponseAsync(Update update)
	{
		var chatId = update.Message.Chat.Id;

		try
		{
			Guard.Against.Null(update.Message);
			Guard.Against.Null(update.Message.From);
			Guard.Against.Null(update.Message.WebAppData);
			Guard.Against.Null(update.Message.WebAppData.Data);

			var createOrderRequest = JsonConvert.DeserializeObject<CreateOrderRequest>(update.Message.WebAppData.Data);

			createOrderRequest!.TelegramUserUID = update.Message.From.Id;

			var createOrderResponse = await _orderService.CreateAsync(createOrderRequest, cancellationToken: CancellationToken.None);

			if (createOrderResponse.Status != ResponseStatus.Success)
			{
				_logger.LogError("Unable to create order. {createOrderResponse}", createOrderResponse);

				await _telegramBot.SendTextMessageAsync(
					chatId,
					await _applicationContentStore.GetValueAsync(ApplicationContentKey.Order.CreateErrorMessage, CancellationToken.None),
					parseMode: ParseMode.Html
				);
				return;
			}

			await _paymentMethodsSender.SendProceedToPaymentAsync(chatId, createOrderResponse.CreatedOrder, CancellationToken.None);
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, exception.Message);
			await _telegramBot.SendDefaultErrorMessageAsync(chatId, _applicationContentStore, _logger, CancellationToken.None);
		}
	}

	public Task<bool> IsResponsibleForUpdateAsync(Update update)
	{
		return Task.FromResult(update.Message?.Type == MessageType.WebAppData);
	}
}
