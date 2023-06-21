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
    private readonly PaymentMethodsSender _paymentMethodsSender;
    private readonly BotContentAppsettings _botContentAppsettings;

    public ShowActiveOrderCommand(
        ITelegramBotClient telegramBot,
        IOrderService orderService,
        PaymentMethodsSender paymentMethodsSender,
        BotContentAppsettings botContentAppsettings)
    {
        _telegramBot = telegramBot;
        _orderService = orderService;
        _paymentMethodsSender = paymentMethodsSender;
        _botContentAppsettings = botContentAppsettings;
    }

    public async Task SendResponseAsync(Update update)
    {
        var chatId = update.Message.Chat.Id;

        var getOrdersResponse = await _orderService.GetUnpaidOrderByTelegramId(chatId, CancellationToken.None);

        if (getOrdersResponse.Data != null)
        {
            // TODO: Send formatted order cart items
            await _paymentMethodsSender.SendEnabledPaymentMethodsAsync(chatId, CancellationToken.None);
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
