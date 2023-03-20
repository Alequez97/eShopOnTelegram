using eShopOnTelegram.TelegramBot.Interfaces;

namespace eShopOnTelegram.TelegramBot.Commands;

public class WebAppCommand : ITelegramCommand
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<WebAppCommand> _logger;
    private readonly ITelegramBotClient _telegramBotClient;
    //private readonly IBasketService _basketService;
    //private readonly EmojiProvider _emojiProvider;
    //private readonly TelegramInvoiceSender _invoiceSender;

    public WebAppCommand(
        IConfiguration configuration,
        ILogger<WebAppCommand> logger,
        ITelegramBotClient telegramBotClient)
        //IBasketService basketService,
        //EmojiProvider emojiProvider,
        //TelegramInvoiceSender invoiceSender)
    {
        _configuration = configuration;
        _logger = logger;
        _telegramBotClient = telegramBotClient;
        //_basketService = basketService;
        //_emojiProvider = emojiProvider;
        //_invoiceSender = invoiceSender;
    }

    public async Task SendResponseAsync(Update update)
    {
        var chatId = update.Message.Chat.Id;
        
        try
        {
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