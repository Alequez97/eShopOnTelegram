using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Keys;
using eShopOnTelegram.Shop.Worker.Constants;
using eShopOnTelegram.Shop.Worker.Services.Telegram.Buttons.Inline.Payment.Interfaces;
using eShopOnTelegram.Utils.Configuration;

using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.Shop.Worker.Services.Telegram.Buttons.Inline.Payment;

public class PaymentThroughSellerTelegramButtonProvider : IPaymentTelegramButtonProvider
{
	private readonly IApplicationContentStore _applicationContentStore;
	private readonly AppSettings _appSettings;

	public PaymentThroughSellerTelegramButtonProvider(IApplicationContentStore applicationContentStore, AppSettings appSettings)
	{
		_applicationContentStore = applicationContentStore;
		_appSettings = appSettings;
	}

	public async Task<InlineKeyboardButton> GetInvoiceGenerationButtonAsync(CancellationToken cancellationToken)
	{
		var buttonText = await _applicationContentStore.GetValueAsync(ApplicationContentKey.Payment.PaymentThroughSeller, cancellationToken);
		var button = InlineKeyboardButton.WithCallbackData(text: buttonText, callbackData: PaymentMethodConstants.PaymentThroughSeller);

		return button;
	}

	public bool PaymentMethodEnabled() => _appSettings.PaymentSettings.PaymentThroughSellerEnabled;
}
