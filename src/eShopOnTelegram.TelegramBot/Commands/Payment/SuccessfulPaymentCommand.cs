using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.Persistence.Entities;
using eShopOnTelegram.TelegramBot.Commands.Interfaces;

namespace eShopOnTelegram.TelegramBot.Commands.Payment;

public class SuccessfulPaymentCommand : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IOrderService _orderService;

    public SuccessfulPaymentCommand(
        ITelegramBotClient telegramBotClient,
        IOrderService orderService)
    {
        _telegramBotClient = telegramBotClient;
        _orderService = orderService;
    }

    public async Task SendResponseAsync(Update update)
    {
        var chatId = update.Message.Chat.Id;
        var orderNumber = update.Message.SuccessfulPayment.InvoicePayload;

        var response = await _orderService.UpdateStatusAsync(orderNumber, OrderStatus.Paid, CancellationToken.None);

        if (response.Status == ResponseStatus.Success)
        {
            await _telegramBotClient.SendTextMessageAsync(
                chatId,
                "Thank you for purchase. We will contact you soon"
            );

            // TODO: Send notification to shop owner, that new order received
        }
        else
        {
            await _telegramBotClient.SendTextMessageAsync(
                chatId,
                "Error during order confirmation. Please contact support",
                ParseMode.MarkdownV2
            );
        }
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