using Ardalis.GuardClauses;

using eShopOnTelegram.Notifications.Interfaces;
using eShopOnTelegram.Persistence.Entities.Orders;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Keys;
using eShopOnTelegram.Shop.Worker.Commands.Interfaces;
using eShopOnTelegram.Shop.Worker.Extensions;

namespace eShopOnTelegram.Shop.Worker.Commands.Payment;

public class SuccessfulPaymentCommand : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBot;
    private readonly IPaymentService _paymentService;
    //private readonly IOrderService _orderService;
    private readonly IApplicationContentStore _applicationContentStore;
    private readonly IEnumerable<INotificationSender> _notificationSenders;
    private readonly ILogger<SuccessfulPaymentCommand> _logger;

    public SuccessfulPaymentCommand(
        ITelegramBotClient telegramBot,
        //IOrderService orderService,
        IPaymentService paymentService,
        IApplicationContentStore applicationContentStore,
        IEnumerable<INotificationSender> notificationSenders,
        ILogger<SuccessfulPaymentCommand> logger)
    {
        _telegramBot = telegramBot;
        _paymentService = paymentService;
        //_orderService = orderService;
        _applicationContentStore = applicationContentStore;
        _notificationSenders = notificationSenders;
        _logger = logger;
    }

    public async Task SendResponseAsync(Update update)
    {
        var chatId = update.Message.Chat.Id;

        try
        {
            Guard.Against.Null(update.Message);
            Guard.Against.Null(update.Message.SuccessfulPayment);
            var orderNumber = update.Message.SuccessfulPayment.InvoicePayload;

            await _paymentService.ConfirmOrderPayment(orderNumber, OrderPaymentMethod.Card);

            await _telegramBot.SendTextMessageAsync(
                chatId,
                await _applicationContentStore.GetValueAsync(ApplicationContentKey.Payment.SuccessfullPayment, CancellationToken.None)
            );

            foreach (var notificationSender in _notificationSenders)
            {
                await notificationSender.SendOrderReceivedNotificationAsync(orderNumber, CancellationToken.None);
            }

            //var response = await _orderService.UpdateStatusAsync(orderNumber, OrderStatus.Paid, CancellationToken.None);

            //if (response.Status == ResponseStatus.Success)
            //{
            //await _telegramBot.SendTextMessageAsync(
            //    chatId,
            //    await _applicationContentStore.GetValueAsync(ApplicationContentKey.Payment.SuccessfullPayment, CancellationToken.None)
            //);

            //foreach (var notificationSender in _notificationSenders)
            //{
            //    await notificationSender.SendOrderReceivedNotificationAsync(orderNumber, CancellationToken.None);
            //}
            //}
            //else
            //{
            //await _telegramBot.SendTextMessageAsync(
            //    chatId,
            //    await _applicationContentStore.GetValueAsync(ApplicationContentKey.Payment.ErrorDuringPaymentConfirmation, CancellationToken.None),
            //    parseMode: ParseMode.Html
            //);
            //}
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            await _telegramBot.SendDefaultErrorMessageAsync(chatId, _applicationContentStore, _logger, CancellationToken.None);
        }
    }

    public Task<bool> IsResponsibleForUpdateAsync(Update update)
    {
        return Task.FromResult(update.Message?.Type == MessageType.SuccessfulPayment);
    }
}