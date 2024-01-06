using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Keys;
using eShopOnTelegram.Shop.Worker.Commands.Interfaces;

namespace eShopOnTelegram.Shop.Worker.Commands.Payment;

public class PreCheckoutQueryCommand : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBot;
    private readonly IOrderService _orderService;
    private readonly IApplicationContentStore _applicationContentStore;

    public PreCheckoutQueryCommand(
        ITelegramBotClient telegramBot,
        IOrderService orderService,
        IApplicationContentStore applicationContentStore
        )
    {
        _telegramBot = telegramBot;
        _orderService = orderService;
        _applicationContentStore = applicationContentStore;
    }

    public async Task SendResponseAsync(Update update)
    {
        var preCheckoutQuery = update.PreCheckoutQuery;

        var getOrderResponse = await _orderService.GetUnpaidOrderByTelegramIdAsync(preCheckoutQuery.From.Id, CancellationToken.None);
        if (getOrderResponse.Status == ResponseStatus.Success)
        {
            if (getOrderResponse.Data.TotalPrice * 100 != update.PreCheckoutQuery.TotalAmount)
            {
                // This is case when user generated multiple invoices in chat 
                // and by mistake choose incorrect invoice and will pay less or more, than in active order

                await _telegramBot.SendTextMessageAsync(preCheckoutQuery.From.Id, await _applicationContentStore.GetValueAsync(ApplicationContentKey.Payment.IncorrectInvoiceChoosen, CancellationToken.None));
                return;
            }

            await _telegramBot.AnswerPreCheckoutQueryAsync(preCheckoutQuery.Id);
            return;
        }

        await _telegramBot.SendTextMessageAsync(preCheckoutQuery.From.Id, await _applicationContentStore.GetValueAsync(ApplicationContentKey.Order.AlreadyPaidOrExpired, CancellationToken.None));
    }

    public Task<bool> IsResponsibleForUpdateAsync(Update update)
    {
        return Task.FromResult(update.PreCheckoutQuery != null);
    }
}
