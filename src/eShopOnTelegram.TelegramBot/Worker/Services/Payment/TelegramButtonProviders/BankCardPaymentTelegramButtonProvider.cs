using eShopOnTelegram.TelegramBot.Worker.Constants;
using eShopOnTelegram.TelegramBot.Worker.Services.Payment.Interfaces;

using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.TelegramBot.Worker.Services.Payment.TelegramButtonProviders;

public class BankCardPaymentTelegramButtonProvider : IPaymentTelegramButtonProvider
{
    public InlineKeyboardButton GetInvoiceGenerationButton()
    {
        return InlineKeyboardButton.WithCallbackData(text: "Pay with bank card", callbackData: PaymentMethodConstants.BankCard);
    }

    public bool PaymentMethodEnabled(PaymentSettings paymentAppsettings)
    {
        return paymentAppsettings.Card.Enabled;
    }
}
