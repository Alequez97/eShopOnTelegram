using eShopOnTelegram.TelegramBot.Appsettings;
using eShopOnTelegram.TelegramBot.Constants;
using eShopOnTelegram.TelegramBot.Services.Payment.Interfaces;

using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.TelegramBot.Services.Payment;

public class BankCardPaymentTelegramButtonProvider : IPaymentTelegramButtonProvider
{
    public InlineKeyboardButton GetInvoiceGenerationButton()
    {
        return InlineKeyboardButton.WithCallbackData(text: "Pay with bank card", callbackData: PaymentMethodConstants.BankCard);
    }

    public bool PaymentMethodEnabled(PaymentAppsettings paymentAppsettings)
    {
        return paymentAppsettings.Card.Enabled;
    }
}
