using Ardalis.GuardClauses;

using eShopOnTelegram.Domain.Requests.Orders;
using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.Persistence.Entities;
using eShopOnTelegram.TelegramBot.Interfaces;

using Microsoft.EntityFrameworkCore.Metadata.Conventions;

using Newtonsoft.Json;



namespace eShopOnTelegram.TelegramBot.Commands;

public class WebAppCommand : ITelegramCommand
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<WebAppCommand> _logger;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IOrderService _orderService;

    //private readonly IBasketService _basketService;
    //private readonly EmojiProvider _emojiProvider;
    //private readonly TelegramInvoiceSender _invoiceSender;

    public WebAppCommand(
        IConfiguration configuration,
        ILogger<WebAppCommand> logger,
        ITelegramBotClient telegramBotClient,
        IOrderService orderService)
        //IBasketService basketService,
        //EmojiProvider emojiProvider,
        //TelegramInvoiceSender invoiceSender)
    {
        _configuration = configuration;
        _logger = logger;
        _telegramBotClient = telegramBotClient;
        _orderService = orderService;

        //_basketService = basketService;
        //_emojiProvider = emojiProvider;
        //_invoiceSender = invoiceSender;
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

            var createOrderResponse = await _orderService.CreateOrder(createOrderRequest);

            if(createOrderResponse.Status != ResponseStatus.Success)
            {
                _logger.LogError("Unable to handle order.");
                await _telegramBotClient.SendTextMessageAsync(
                    chatId,
                    $"Unable to handle your order. Sorry!!!",
                    ParseMode.MarkdownV2
                );
            }

            // todo generate invoice
            // todo send email with generated invoice

            //var addItemsToBasketRequest = JsonConvert.DeserializeObject<AddItemsToBasketRequest>(update.Message.WebAppData.Data);
            //addItemsToBasketRequest.TelegramId = chatId.ToString();

            //var createOrderResponse = await _basketService.AddItemsAsync(addItemsToBasketRequest);

            //if (createOrderResponse.Status == ResponseStatus.Success)
            //{
            //    await _invoiceSender.SendConfiguredInvoice(chatId, addItemsToBasketRequest.BasketItems);
            //    return;
            //}
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
    public required IList<CartItem> CartItems { get; set; }
}