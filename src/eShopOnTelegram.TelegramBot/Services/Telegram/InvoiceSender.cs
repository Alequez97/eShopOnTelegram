using eShopOnTelegram.Domain.Requests.Orders;
using eShopOnTelegram.TelegramBot.Appsettings;

namespace eShopOnTelegram.TelegramBot.Services.Telegram;

public class InvoiceSender
{
    private readonly BotContentAppsettings _botContentAppsettings;
    private readonly PaymentAppsettings _paymentAppsettings;

    public InvoiceSender(
        BotContentAppsettings botContentAppsettings,
        PaymentAppsettings paymentAppsettings)
    {
        _botContentAppsettings = botContentAppsettings;
        _paymentAppsettings = paymentAppsettings;
    }

    public async Task SendInvoiceAsync(IEnumerable<CreateCartItemRequest> cartItems)
    {
        await Console.Out.WriteLineAsync(cartItems.ToString());

        if (!_paymentAppsettings.AllPaymentsDisabled)
        {
            await Console.Out.WriteLineAsync(_botContentAppsettings.SendInvoiceText);
        }
    }
}
