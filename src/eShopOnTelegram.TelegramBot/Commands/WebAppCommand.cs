using Ardalis.GuardClauses;

using eShopOnTelegram.Domain.Requests.Orders;
using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.TelegramBot.Commands.Interfaces;
using eShopOnTelegram.TelegramBot.Services.Telegram;

using Newtonsoft.Json;

namespace eShopOnTelegram.TelegramBot.Commands;

public class WebAppCommand : ITelegramCommand
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<WebAppCommand> _logger;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IOrderService _orderService;
    private readonly InvoiceSender _invoiceSender;

    public WebAppCommand(
        IConfiguration configuration,
        ILogger<WebAppCommand> logger,
        ITelegramBotClient telegramBotClient,
        IOrderService orderService,
        InvoiceSender invoiceSender)
    {
        _configuration = configuration;
        _logger = logger;
        _telegramBotClient = telegramBotClient;
        _orderService = orderService;
        _invoiceSender = invoiceSender;
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
                _logger.LogError("Unable to handle order.");
                await _telegramBotClient.SendTextMessageAsync(
                    chatId,
                    $"Unable to handle your order. Sorry!!!",
                    ParseMode.MarkdownV2
                );
                return;
            }

            await _invoiceSender.SendInvoiceAsync(createOrderRequest.CartItems, chatId, createOrderResponse.OrderNumber, CancellationToken.None);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception in web app telegram command");

            await _telegramBotClient.SendTextMessageAsync(
                chatId,
                "Ошибка! Пожалуйста, обратитесь в команду поддержки.",
                ParseMode.MarkdownV2
            );
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