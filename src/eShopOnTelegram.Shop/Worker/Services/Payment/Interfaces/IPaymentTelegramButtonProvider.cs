using eShopOnTelegram.Utils.Configuration;

using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.Shop.Worker.Services.Payment.Interfaces;

public interface IPaymentTelegramButtonProvider
{
	InlineKeyboardButton GetInvoiceGenerationButton();

	bool PaymentMethodEnabled(PaymentSettings paymentAppsettings);
}
