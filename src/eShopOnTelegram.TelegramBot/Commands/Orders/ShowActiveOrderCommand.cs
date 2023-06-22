using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.TelegramBot.Appsettings;
using eShopOnTelegram.TelegramBot.Commands.Interfaces;
using eShopOnTelegram.TelegramBot.Constants;
using eShopOnTelegram.TelegramBot.Extensions;
using eShopOnTelegram.TelegramBot.Services.Telegram;

namespace eShopOnTelegram.TelegramBot.Commands.Orders;

public class ShowActiveOrderCommand : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBot;
    private readonly IOrderService _orderService;
    private readonly PaymentProceedMessageSender _paymentProceedMessage;
    private readonly BotContentAppsettings _botContentAppsettings;

    public ShowActiveOrderCommand(
        ITelegramBotClient telegramBot,
        IOrderService orderService,
        PaymentProceedMessageSender paymentProceedMessageSender,
        BotContentAppsettings botContentAppsettings)
    {
        _telegramBot = telegramBot;
        _orderService = orderService;
        _paymentProceedMessage = paymentProceedMessageSender;
        _botContentAppsettings = botContentAppsettings;
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
            await _telegramBot.SendTextMessageAsync(chatId, _botContentAppsettings.Order.NoUnpaidOrderFound.OrNextIfNullOrEmpty(BotContentDefaultConstants.Order.NoUnpaidOrderFound));
        }
    }

    public bool IsResponsibleForUpdate(Update update)
    {
        return update.Message?.Text?.Contains(_botContentAppsettings.Order.ShowUnpaidOrder.OrNextIfNullOrEmpty(BotContentDefaultConstants.Order.ShowUnpaidOrder)) ?? false;
    }
}
