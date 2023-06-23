using Ardalis.GuardClauses;

using eShopOnTelegram.ApplicationContent.Interfaces;
using eShopOnTelegram.ApplicationContent.Keys;
using eShopOnTelegram.Domain.Requests.Orders;
using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.TelegramBot.Commands.Interfaces;
using eShopOnTelegram.TelegramBot.Extensions;
using eShopOnTelegram.TelegramBot.Services.Telegram;

using Newtonsoft.Json;

namespace eShopOnTelegram.TelegramBot.Commands.Order;

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
                    await _applicationContentStore.GetSingleValueAsync(ApplicationContentKey.Order.CreateErrorMessage, CancellationToken.None),
                    parseMode: ParseMode.Html
                );
                return;
            }

            await _paymentMethodsSender.SendProceedToPaymentAsync(chatId, createOrderResponse.CreatedOrder, CancellationToken.None);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            await _telegramBot.SendDefaultErrorMessageAsync(chatId, _applicationContentStore, CancellationToken.None);
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