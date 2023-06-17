using eShopOnTelegram.TelegramBot.Appsettings;
using eShopOnTelegram.TelegramBot.Constants;
using eShopOnTelegram.TelegramBot.Services.Payment.Interfaces;

using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.TelegramBot.Services.Payment;

public class PlicioPaymentTelegramButtonProvider : IPaymentTelegramButtonProvider
{
    public InlineKeyboardButton GetInvoiceGenerationButton()
    {
        return InlineKeyboardButton.WithCallbackData(text: "Pay with Plicio (crypto)", callbackData: PaymentMethodConstants.Plicio);
    }

    public bool PaymentMethodEnabled(PaymentAppsettings paymentAppsettings)
    {
        return paymentAppsettings.Plisio.Enabled;
    }
}
