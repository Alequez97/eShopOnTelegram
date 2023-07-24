using eShopOnTelegram.TelegramBot.Worker.Appsettings;
using eShopOnTelegram.TelegramBot.Worker.Constants;
using eShopOnTelegram.TelegramBot.Worker.Services.Payment.Interfaces;

using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.TelegramBot.Worker.Services.Payment.TelegramButtonProviders;

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
