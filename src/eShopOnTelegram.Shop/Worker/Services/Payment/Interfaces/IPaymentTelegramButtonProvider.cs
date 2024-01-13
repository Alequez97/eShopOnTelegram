using eShopOnTelegram.Utils.Configuration;

using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.Shop.Worker.Services.Payment.Interfaces;

public interface IPaymentTelegramButtonProvider
{
	Task<InlineKeyboardButton> GetInvoiceGenerationButtonAsync(CancellationToken cancellationToken);

	bool PaymentMethodEnabled(PaymentSettings paymentAppsettings);
}
