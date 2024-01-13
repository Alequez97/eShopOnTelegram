using eShopOnTelegram.Shop.Worker.Constants;
using eShopOnTelegram.Shop.Worker.Services.Payment.Interfaces;
using eShopOnTelegram.Utils.Configuration;

using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.Shop.Worker.Services.Payment.TelegramButtonProviders;

public class PlisioPaymentTelegramButtonProvider : IPaymentTelegramButtonProvider
{
	public async Task<InlineKeyboardButton> GetInvoiceGenerationButtonAsync(CancellationToken cancellationToken)
	{
		// TODO: Replace button text with text from ApplicationContentStore when Plisio integration is fully implemented
		var buttonText = "Pay with Plisio (crypto)";
		var button = InlineKeyboardButton.WithCallbackData(text: buttonText, callbackData: PaymentMethodConstants.Plisio);

		return await Task.FromResult(button);
	}

	public bool PaymentMethodEnabled(PaymentSettings paymentAppsettings)
	{
		return paymentAppsettings.Plisio.Enabled;
	}
}
