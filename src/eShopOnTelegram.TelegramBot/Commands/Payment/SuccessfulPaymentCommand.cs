using eShopOnTelegram.ApplicationContent.Interfaces;
using eShopOnTelegram.ApplicationContent.Keys;
using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.Persistence.Entities;
using eShopOnTelegram.TelegramBot.Commands.Interfaces;
using eShopOnTelegram.TelegramBot.Extensions;

namespace eShopOnTelegram.TelegramBot.Commands.Payment;

public class SuccessfulPaymentCommand : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBot;
    private readonly IOrderService _orderService;
    private readonly IApplicationContentStore _applicationContentStore;
    private readonly ILogger<SuccessfulPaymentCommand> _logger;

    public SuccessfulPaymentCommand(
        ITelegramBotClient telegramBot,
        IOrderService orderService,
        IApplicationContentStore applicationContentStore,
        ILogger<SuccessfulPaymentCommand> logger)
    {
        _telegramBot = telegramBot;
        _orderService = orderService;
        _applicationContentStore = applicationContentStore;
        _logger = logger;
    }

    public async Task SendResponseAsync(Update update)
    {
        var chatId = update.Message.Chat.Id;

        try
        {
            var orderNumber = update.Message.SuccessfulPayment.InvoicePayload;

            var response = await _orderService.UpdateStatusAsync(orderNumber, OrderStatus.Paid, CancellationToken.None);

            if (response.Status == ResponseStatus.Success)
            {
                await _telegramBot.SendTextMessageAsync(
                    chatId,
                    await _applicationContentStore.GetSingleValueAsync(ApplicationContentKey.Payment.SuccessfullPayment, CancellationToken.None)
                );

                // TODO: Send notification to shop owner, that new order received
            }
            else
            {
                await _telegramBot.SendTextMessageAsync(
                    chatId,
                    await _applicationContentStore.GetSingleValueAsync(ApplicationContentKey.Payment.ErrorDuringPaymentConfirmation, CancellationToken.None),
                    parseMode: ParseMode.Html
                );
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            await _telegramBot.SendDefaultErrorMessageAsync(chatId, _applicationContentStore, CancellationToken.None);
        }
    }

    public Task<bool> IsResponsibleForUpdateAsync(Update update)
    {
        return Task.FromResult(update.Message?.Type == MessageType.SuccessfulPayment);
    }
}