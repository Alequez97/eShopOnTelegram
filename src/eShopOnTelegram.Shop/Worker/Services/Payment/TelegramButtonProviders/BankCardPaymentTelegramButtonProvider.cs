using eShopOnTelegram.Shop.Worker.Constants;
using eShopOnTelegram.Shop.Worker.Services.Payment.Interfaces;
using eShopOnTelegram.Utils.Configuration;

using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.Shop.Worker.Services.Payment.TelegramButtonProviders;

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
