using Ardalis.GuardClauses;

using eShopOnTelegram.Domain.Requests.Orders;
using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Keys;
using eShopOnTelegram.TelegramBot.Worker.Commands.Interfaces;
using eShopOnTelegram.TelegramBot.Worker.Extensions;
using eShopOnTelegram.TelegramBot.Worker.Services.Telegram;

using Newtonsoft.Json;

namespace eShopOnTelegram.TelegramBot.Worker.Commands.Orders;

/// <summary>
/// <para>Currently this command is used to create new order.
/// This command is executed when sendData(data: string) function is executed in web app</para>
/// <i>Note: sendData() function is available only when web app is opened with telegram keyboard button</i>
/// </summary>
public class WebAppCommand : ITelegramCommand
{
    private readonly ILogger<WebAppCommand> _logger;
    private readonly ITelegramBotClient _telegramBot;
    private readonly IOrderService _orderService;
    private readonly PaymentProceedMessageSender _paymentMethodsSender;
    private readonly IApplicationContentStore _applicationContentStore;

    public WebAppCommand(
        ILogger<WebAppCommand> logger,
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

            var webAppData = JsonConvert.DeserializeObject<WebAppCommandData>(update.Message.WebAppData.Data);

            var createOrderRequest = new CreateOrderRequest()
            {
                TelegramUserUID = update.Message.From.Id,
                CartItems = webAppData.CartItems,
            };

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

public class WebAppCommandData
{
    public required IList<CreateCartItemRequest> CartItems { get; set; }
}