using eShopOnTelegram.Domain.Requests.Orders;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.TelegramBot.Appsettings;
using eShopOnTelegram.TelegramBot.Extensions;

namespace eShopOnTelegram.TelegramBot.Services.Telegram;

public class InvoiceSender
{
    private readonly ITelegramBotClient _telegramBot;
    private readonly IProductService _productService;
    private readonly BotContentAppsettings _botContentAppsettings;
    private readonly PaymentAppsettings _paymentAppsettings;

    public InvoiceSender(
        ITelegramBotClient telegramBot,
        IProductService productService,
        BotContentAppsettings botContentAppsettings,
        PaymentAppsettings paymentAppsettings)
    {
        _telegramBot = telegramBot;
        _productService = productService;
        _botContentAppsettings = botContentAppsettings;
        _paymentAppsettings = paymentAppsettings;
    }

    public async Task SendInvoiceAsync(IEnumerable<CreateCartItemRequest> cartItems, long chatId, string orderNumber, CancellationToken cancellationToken)
    {
        await Console.Out.WriteLineAsync(cartItems.ToString());

        if (_paymentAppsettings.AllPaymentsDisabled)
        {
            await _telegramBot.SendTextMessageAsync(chatId, _botContentAppsettings.SendInvoiceText);
            return;
        }

        await _telegramBot.SendInvoiceAsync(
            chatId,
            $"Заказ номер {orderNumber}",
            "Описание",
            orderNumber,
            _paymentAppsettings.Card.ApiToken,
            _paymentAppsettings.Card.Currency,
            await cartItems.GetPaymentLabeledPricesAsync(_productService, cancellationToken),
            needShippingAddress: true,
            needPhoneNumber: true,
            needName: true,
            cancellationToken: cancellationToken
        );
    }
}
