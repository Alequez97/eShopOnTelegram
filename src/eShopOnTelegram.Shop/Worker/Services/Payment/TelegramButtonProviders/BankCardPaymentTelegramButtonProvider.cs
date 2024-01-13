using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Keys;
using eShopOnTelegram.Shop.Worker.Constants;
using eShopOnTelegram.Shop.Worker.Services.Payment.Interfaces;
using eShopOnTelegram.Utils.Configuration;

using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.Shop.Worker.Services.Payment.TelegramButtonProviders;

public class BankCardPaymentTelegramButtonProvider : IPaymentTelegramButtonProvider
{
	private readonly IApplicationContentStore _applicationContentStore;

	public BankCardPaymentTelegramButtonProvider(IApplicationContentStore applicationContentStore)
	{
		_applicationContentStore = applicationContentStore;
	}

	public async Task<InlineKeyboardButton> GetInvoiceGenerationButtonAsync(CancellationToken cancellationToken)
	{
		var buttonText = await _applicationContentStore.GetValueAsync(ApplicationContentKey.Payment.PayWithBankCard, cancellationToken);
		var button = InlineKeyboardButton.WithCallbackData(text: buttonText, callbackData: PaymentMethodConstants.BankCard);

		return button;
	}

	public bool PaymentMethodEnabled(PaymentSettings paymentAppsettings)
	{
		return paymentAppsettings.Card.Enabled;
	}
}
