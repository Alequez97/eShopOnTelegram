using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.TelegramBot.Appsettings;
using eShopOnTelegram.TelegramBot.Commands.Interfaces;
using eShopOnTelegram.TelegramBot.Constants;
using eShopOnTelegram.TelegramBot.Extensions;

namespace eShopOnTelegram.TelegramBot.Commands.Payment;

public class PreCheckoutQueryCommand : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBot;
    private readonly IOrderService _orderService;
    private readonly BotContentAppsettings _botContentAppsettings;

    public PreCheckoutQueryCommand(
        ITelegramBotClient telegramBot,
        IOrderService orderService,
        BotContentAppsettings botContentAppsettings
        )
    {
        _telegramBot = telegramBot;
        _orderService = orderService;
        _botContentAppsettings = botContentAppsettings;
    }

    public async Task SendResponseAsync(Update update)
    {
        var preCheckoutQuery = update.PreCheckoutQuery;

        var getOrderResponse = await _orderService.GetUnpaidOrderByTelegramId(preCheckoutQuery.From.Id, CancellationToken.None);
        if (getOrderResponse.Status == ResponseStatus.Success) 
        {
            await _telegramBot.AnswerPreCheckoutQueryAsync(preCheckoutQuery.Id);
            return;
        }

        await _telegramBot.SendTextMessageAsync(preCheckoutQuery.From.Id, _botContentAppsettings.Order.AlreadyPaidOrExpired.OrNextIfNullOrEmpty(BotContentDefaultConstants.Order.AlreadyPaidOrExpired));
    }

    public bool IsResponsibleForUpdate(Update update)
    {
        return update.PreCheckoutQuery != null;
    }
}