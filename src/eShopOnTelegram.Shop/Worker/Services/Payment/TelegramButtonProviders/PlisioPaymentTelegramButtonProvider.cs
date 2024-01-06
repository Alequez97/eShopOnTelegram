using eShopOnTelegram.Shop.Worker.Constants;
using eShopOnTelegram.Shop.Worker.Services.Payment.Interfaces;
using eShopOnTelegram.Utils.Configuration;

using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.Shop.Worker.Services.Payment.TelegramButtonProviders;

public class PlisioPaymentTelegramButtonProvider : IPaymentTelegramButtonProvider
{
	public InlineKeyboardButton GetInvoiceGenerationButton()
	{
		return InlineKeyboardButton.WithCallbackData(text: "Pay with Plisio (crypto)", callbackData: PaymentMethodConstants.Plisio);
	}

	public bool PaymentMethodEnabled(PaymentSettings paymentAppsettings)
	{
		return paymentAppsettings.Plisio.Enabled;
	}
}
