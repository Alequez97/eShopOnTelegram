using Ardalis.GuardClauses;

using eShopOnTelegram.Domain.Requests.Orders;
using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.TelegramBot.Appsettings;
using eShopOnTelegram.TelegramBot.Commands.Interfaces;
using eShopOnTelegram.TelegramBot.Constants;
using eShopOnTelegram.TelegramBot.Extensions;
using eShopOnTelegram.TelegramBot.Services.Telegram;

using Newtonsoft.Json;

namespace eShopOnTelegram.TelegramBot.Commands;

public class WebAppCommand : ITelegramCommand
{
    private readonly ILogger<WebAppCommand> _logger;
    private readonly ITelegramBotClient _telegramBot;
    private readonly IOrderService _orderService;
    private readonly PaymentMethodsSender _paymentMethodsSender;
    private readonly BotContentAppsettings _botContentAppsettings;

    public WebAppCommand(
        ILogger<WebAppCommand> logger,
        ITelegramBotClient telegramBot,
        IOrderService orderService,
        PaymentMethodsSender invoiceSender,
        BotContentAppsettings botContentAppsettings)
    {
        _logger = logger;
        _telegramBot = telegramBot;
        _orderService = orderService;
        _paymentMethodsSender = invoiceSender;
        _botContentAppsettings = botContentAppsettings;
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
                    _botContentAppsettings.Order.CreateErrorMessage ?? BotContentDefaultConstants.Order.CreateErrorMessage,
                    ParseMode.MarkdownV2
                );
                return;
            }

            await _paymentMethodsSender.SendEnabledPaymentMethodsAsync(chatId, CancellationToken.None);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            await _telegramBot.SendCommonErrorMessageAsync(chatId, _botContentAppsettings, CancellationToken.None);
        }
    }

    public bool IsResponsibleForUpdate(Update update)
    {
        return update.Message?.Type == MessageType.WebAppData;
    }
}

public class WebAppCommandData
{
    public required IList<CreateCartItemRequest> CartItems { get; set; }
}