using eShopOnTelegram.TelegramBot.Worker.Appsettings;

using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.TelegramBot.Worker.Services.Payment.Interfaces;

public interface IPaymentTelegramButtonProvider
{
    InlineKeyboardButton GetInvoiceGenerationButton();

    bool PaymentMethodEnabled(PaymentAppsettings paymentAppsettings);
}
