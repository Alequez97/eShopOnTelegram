using eShopOnTelegram.TelegramBot.Worker.Constants;
using eShopOnTelegram.TelegramBot.Worker.Services.Payment.Interfaces;

using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.TelegramBot.Worker.Services.Payment.TelegramButtonProviders;

public class PlisioPaymentTelegramButtonProvider : IPaymentTelegramButtonProvider
{
    public InlineKeyboardButton GetInvoiceGenerationButton()
    {
        return InlineKeyboardButton.WithCallbackData(text: "Pay with Plisio (crypto)", callbackData: PaymentMethodConstants.Plisio);
    }

    public bool PaymentMethodEnabled(PaymentSettings paymentAppsettings)
    {
        return paymentAppsettings.Plisio.Enabled;
    }
}
