using TelegramBot.Commands.Interfaces;

namespace eShopOnTelegram.TelegramBot.Commands.Payment;

public class SuccessfulPaymentCommand : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBotClient;
    //private readonly IOrderService _orderService;

    public SuccessfulPaymentCommand(
        ITelegramBotClient telegramBotClient
        /*IOrderService orderService*/)
    {
        _telegramBotClient = telegramBotClient;
        //_orderService = orderService;
    }

    public async Task SendResponseAsync(Update update)
    {
        var chatId = update.Message.Chat.Id;

        //var request = new CreateOrderRequest() { TelegramId = chatId.ToString() };

        //var response = await _orderService.CreateAsync(request);

        //if (response.Status == ResponseStatus.Success)
        //{
        //    await _telegramBotClient.SendTextMessageAsync(
        //        chatId,
        //        "Спасибо за покупку, перейдите в раздел мои товары, чтобы просмотреть свои покупки",
        //        ParseMode.MarkdownV2
        //    );
        //} 
        //else
        //{
        //    await _telegramBotClient.SendTextMessageAsync(
        //        chatId,
        //        "Ошибка при обработке заказа\\. Обратитесь в команду поддержки",
        //        ParseMode.MarkdownV2
        //    );
        //}
    }

    public bool IsResponsibleForUpdate(Update update)
    {
        return update.Message?.Type == MessageType.SuccessfulPayment;
    }

    private string ReturnNullIfStringNullOrEmpty(string str)
    {
        return string.IsNullOrWhiteSpace(str) ? null : str;
    }
}