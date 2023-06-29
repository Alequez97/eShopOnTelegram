using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Keys;

using Newtonsoft.Json;

namespace eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Content;

public class ApplicationContentModel
{
    [JsonProperty(ApplicationContentKey.TelegramBot.StartError)]
    public string TelegramBot_StartError { get; set; } = "Error. Try again later";

    [JsonProperty(ApplicationContentKey.TelegramBot.WelcomeText)]
    public string TelegramBot_WelcomeText { get; set; } = "Welcome to our shop";

    [JsonProperty(ApplicationContentKey.TelegramBot.OpenShopButtonText)]
    public string TelegramBot_OpenShopButtonText { get; set; } = "*** Click here to open shop ***";

    [JsonProperty(ApplicationContentKey.TelegramBot.UnknownCommandText)]
    public string TelegramBot_UnknownCommandText { get; set; } = "Unknown command was sent";

    [JsonProperty(ApplicationContentKey.TelegramBot.DefaultErrorMessage)]
    public string TelegramBot_DefaultErrorMessage { get; set; } = "Error. Something went wrong. Please try again later";

    [JsonProperty(ApplicationContentKey.Order.ShowUnpaidOrder)]
    public string Order_ShowUnpaidOrder { get; set; } = "Show unpaid order";

    [JsonProperty(ApplicationContentKey.Order.NoUnpaidOrderFound)]
    public string Order_NoUnpaidOrderFound { get; set; } = "You don't have unpaid orders";

    [JsonProperty(ApplicationContentKey.Order.AlreadyPaidOrExpired)]
    public string Order_AlreadyPaidOrExpired { get; set; } = "You already paid for this order, or time to accomplish expired. Try again to create a new order";

    [JsonProperty(ApplicationContentKey.Order.CreateErrorMessage)]
    public string Order_CreateErrorMessage { get; set; } = "Error during order creation";

    [JsonProperty(ApplicationContentKey.Order.InvoiceGenerationFailedErrorMessage)]
    public string Order_InvoiceGenerationFailedErrorMessage { get; set; } = "Error during invoice generation. Try again later";

    [JsonProperty(ApplicationContentKey.Order.OrderNumberTitle)]
    public string Order_OrderNumberTitle { get; set; } = "Order number";

    [JsonProperty(ApplicationContentKey.Order.OrderSummaryTitle)]
    public string Order_OrderSummaryTitle { get; set; } = "Your order summary";

    [JsonProperty(ApplicationContentKey.Order.TotalPriceTitle)]
    public string Order_TotalPriceTitle { get; set; } = "Total price";

    [JsonProperty(ApplicationContentKey.Payment.NoEnabledPayments)]
    public string Payment_NoEnabledPayments { get; set; } = "Thank you for your purchase";

    [JsonProperty(ApplicationContentKey.Payment.ChoosePaymentMethod)]
    public string Payment_ChoosePaymentMethod { get; set; } = "Please select a payment method";

    [JsonProperty(ApplicationContentKey.Payment.ProceedToPayment)]
    public string Payment_ProceedToPayment { get; set; } = "Proceed to payment";

    [JsonProperty(ApplicationContentKey.Payment.InvoiceReceiveMessage)]
    public string Payment_InvoiceReceiveMessage { get; set; } = "Please receive your invoice";

    [JsonProperty(ApplicationContentKey.Payment.IncorrectInvoiceChoosen)]
    public string Payment_IncorrectInvoiceChoosen { get; set; } = "You chose an incorrect invoice to pay for";

    [JsonProperty(ApplicationContentKey.Payment.SuccessfullPayment)]
    public string Payment_SuccessfullPayment { get; set; } = "Thank you for your purchase";

    [JsonProperty(ApplicationContentKey.Payment.ErrorDuringPaymentConfirmation)]
    public string Payment_ErrorDuringPaymentConfirmation { get; set; } = "Error during order confirmation";

}
