using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.Shop.Worker.Services.Telegram.Buttons.Inline.Payment.Interfaces;

public interface IPaymentTelegramButtonProvider
{
	Task<InlineKeyboardButton> GetInvoiceGenerationButtonAsync(CancellationToken cancellationToken);

	bool PaymentMethodEnabled();
}
