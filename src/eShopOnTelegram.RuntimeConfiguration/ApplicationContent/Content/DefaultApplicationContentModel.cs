using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Keys;

using Newtonsoft.Json;

namespace eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Content;

public class DefaultApplicationContentModel
{
	// TELEGRAM BOT
	[JsonProperty(ApplicationContentKey.TelegramBot.WelcomeText)]
	public string TelegramBot_WelcomeText => "Welcome to our shop <tg-spoiler>Spoiler</tg-spoiler>";

	[JsonProperty(ApplicationContentKey.TelegramBot.OpenShopButtonText)]
	public string TelegramBot_OpenShopButtonText => "*** Click here to open shop ***";

	[JsonProperty(ApplicationContentKey.TelegramBot.UnknownCommandText)]
	public string TelegramBot_UnknownCommandText => "Unknown command was sent";

	[JsonProperty(ApplicationContentKey.TelegramBot.DefaultErrorMessage)]
	public string TelegramBot_DefaultErrorMessage => "Error. Something went wrong. Please try again later";

	// ORDER

	[JsonProperty(ApplicationContentKey.Order.ShowUnpaidOrder)]
	public string Order_ShowUnpaidOrder => "Show unpaid order";

	[JsonProperty(ApplicationContentKey.Order.TotalPriceTitle)]
	public string Order_TotalPriceTitle => "Total price";

	[JsonProperty(ApplicationContentKey.Order.PaymentMethodAlreadySelected)]
	public string Order_PaymentMethodAlreadySelected => "Payment method already selected";

	// PAYMENT

	[JsonProperty(ApplicationContentKey.Payment.NoEnabledPayments)]
	public string Payment_NoEnabledPayments => "Thank you for your purchase";

	[JsonProperty(ApplicationContentKey.Payment.InvoiceReceiveMessage)]
	public string Payment_InvoiceReceiveMessage => "Please receive your invoice";

	[JsonProperty(ApplicationContentKey.Payment.SuccessfullPayment)]
	public string Payment_SuccessfullPayment => "Thank you for your purchase";

	// TODO: ADD SHOP ADMINISTRATOR NAME (LINK)
	[JsonProperty(ApplicationContentKey.Payment.PaymentThroughSeller)]
	public string Payment_PaymentThroughSeller => "Please contact shop administrator to discuss further payment details";
}
