using eShopOnTelegram.TelegramBot.Appsettings;

using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.TelegramBot.Services.Payment.Interfaces;

public interface IPaymentTelegramButtonProvider
{
    InlineKeyboardButton GetInvoiceGenerationButton();

    bool PaymentMethodEnabled(PaymentAppsettings paymentAppsettings);
}
