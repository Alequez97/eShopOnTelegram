using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Keys;
using eShopOnTelegram.TelegramBot.Worker.Commands.Interfaces;
using eShopOnTelegram.TelegramBot.Worker.Services.Telegram;

namespace eShopOnTelegram.TelegramBot.Worker.Commands.Orders;

public class ShowActiveOrderCommand : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBot;
    private readonly IOrderService _orderService;
    private readonly PaymentProceedMessageSender _paymentProceedMessage;
    private readonly IApplicationContentStore _applicationContentStore;

    public ShowActiveOrderCommand(
        ITelegramBotClient telegramBot,
        IOrderService orderService,
        PaymentProceedMessageSender paymentProceedMessageSender,
        IApplicationContentStore applicationContentStore)
    {
        _telegramBot = telegramBot;
        _orderService = orderService;
        _paymentProceedMessage = paymentProceedMessageSender;
        _applicationContentStore = applicationContentStore;
    }

    public async Task SendResponseAsync(Update update)
    {
        var chatId = update.Message.Chat.Id;

        var getOrdersResponse = await _orderService.GetUnpaidOrderByTelegramIdAsync(chatId, CancellationToken.None);

        if (getOrdersResponse.Data != null)
        {
            await _paymentProceedMessage.SendProceedToPaymentAsync(chatId, getOrdersResponse.Data, CancellationToken.None);
        }
        else
        {
            await _telegramBot.SendTextMessageAsync(chatId, await _applicationContentStore.GetValueAsync(ApplicationContentKey.Order.NoUnpaidOrderFound, CancellationToken.None));
        }
    }

    public async Task<bool> IsResponsibleForUpdateAsync(Update update)
    {
        return update.Message?.Text?.Contains(await _applicationContentStore.GetValueAsync(ApplicationContentKey.Order.ShowUnpaidOrder, CancellationToken.None)) ?? false;
    }
}
