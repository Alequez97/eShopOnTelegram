using eShopOnTelegram.Shop.Worker.Constants;
using eShopOnTelegram.Shop.Worker.Services.Payment.Interfaces;
using eShopOnTelegram.Utils.Configuration;

using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.Shop.Worker.Services.Payment.TelegramButtonProviders;

public class PaymentThroughSellerTelegramButtonProvider : IPaymentTelegramButtonProvider
{
	public InlineKeyboardButton GetInvoiceGenerationButton()
	{
		return InlineKeyboardButton.WithCallbackData(text: "Discuss payment options with seller", callbackData: PaymentMethodConstants.PaymentThroughSeller);
	}

	public bool PaymentMethodEnabled(PaymentSettings paymentAppsettings) => paymentAppsettings.PaymentThroughSellerEnabled;
}
