using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.TelegramBot.Worker.Services.Payment.Interfaces;

public interface IPaymentTelegramButtonProvider
{
    InlineKeyboardButton GetInvoiceGenerationButton();

    bool PaymentMethodEnabled(PaymentSettings paymentAppsettings);
}
