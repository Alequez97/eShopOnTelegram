using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Keys;

using Newtonsoft.Json;

namespace eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Content;

public class DefaultApplicationContentModel
{
    // TELEGRAM BOT

    [JsonProperty(ApplicationContentKey.TelegramBot.StartError)]
    public string TelegramBot_StartError => "Error. Try again later";

    [JsonProperty(ApplicationContentKey.TelegramBot.WelcomeText)]
    public string TelegramBot_WelcomeText => "Welcome to our shop";

    [JsonProperty(ApplicationContentKey.TelegramBot.OpenShopButtonText)]
    public string TelegramBot_OpenShopButtonText => "*** Click here to open shop ***";

    [JsonProperty(ApplicationContentKey.TelegramBot.UnknownCommandText)]
    public string TelegramBot_UnknownCommandText => "Unknown command was sent";

    [JsonProperty(ApplicationContentKey.TelegramBot.DefaultErrorMessage)]
    public string TelegramBot_DefaultErrorMessage => "Error. Something went wrong. Please try again later";

    // ORDER

    [JsonProperty(ApplicationContentKey.Order.ShowUnpaidOrder)]
    public string Order_ShowUnpaidOrder => "Show unpaid order";

    [JsonProperty(ApplicationContentKey.Order.NoUnpaidOrderFound)]
    public string Order_NoUnpaidOrderFound => "You don't have unpaid orders";

    [JsonProperty(ApplicationContentKey.Order.AlreadyPaidOrExpired)]
    public string Order_AlreadyPaidOrExpired => "You already paid for this order, or time to accomplish expired. Try again to create a new order";

    [JsonProperty(ApplicationContentKey.Order.CreateErrorMessage)]
    public string Order_CreateErrorMessage => "Error during order creation";

    [JsonProperty(ApplicationContentKey.Order.InvoiceGenerationFailedErrorMessage)]
    public string Order_InvoiceGenerationFailedErrorMessage => "Error during invoice generation. Try again later";

    [JsonProperty(ApplicationContentKey.Order.OrderNumberTitle)]
    public string Order_OrderNumberTitle => "Order number";

    [JsonProperty(ApplicationContentKey.Order.OrderSummaryTitle)]
    public string Order_OrderSummaryTitle => "Your order summary";

    [JsonProperty(ApplicationContentKey.Order.TotalPriceTitle)]
    public string Order_TotalPriceTitle => "Total price";

    [JsonProperty(ApplicationContentKey.Order.PaymentMethodAlreadySelected)]
    public string Order_PaymentMethodAlreadySelected => "Payment method already selected";

    [JsonProperty(ApplicationContentKey.Order.UnableToGetShippingAddress)]
    public string Order_UnableToGetShippingAddress => "Unable to get shipping address. Please contact bot support team and precise where you want to receive your order";

    // PAYMENT

    [JsonProperty(ApplicationContentKey.Payment.NoEnabledPayments)]
    public string Payment_NoEnabledPayments => "Thank you for your purchase";

    [JsonProperty(ApplicationContentKey.Payment.ChoosePaymentMethod)]
    public string Payment_ChoosePaymentMethod => "Please select a payment method";

    [JsonProperty(ApplicationContentKey.Payment.ProceedToPayment)]
    public string Payment_ProceedToPayment => "Proceed to payment";

    [JsonProperty(ApplicationContentKey.Payment.InvoiceReceiveMessage)]
    public string Payment_InvoiceReceiveMessage => "Please receive your invoice";

    [JsonProperty(ApplicationContentKey.Payment.IncorrectInvoiceChoosen)]
    public string Payment_IncorrectInvoiceChoosen => "You chose an incorrect invoice to pay for";

    [JsonProperty(ApplicationContentKey.Payment.SuccessfullPayment)]
    public string Payment_SuccessfullPayment => "Thank you for your purchase";

    [JsonProperty(ApplicationContentKey.Payment.ErrorDuringPaymentConfirmation)]
    public string Payment_ErrorDuringPaymentConfirmation => "Error during order confirmation";

    // TODO: ADD SHOP ADMINISTRATOR NAME (LINK)
    [JsonProperty(ApplicationContentKey.Payment.PaymentThroughSeller)]
    public string Payment_PaymentThroughSeller => "Please contact shop administrator to discuss further payment details";
}
