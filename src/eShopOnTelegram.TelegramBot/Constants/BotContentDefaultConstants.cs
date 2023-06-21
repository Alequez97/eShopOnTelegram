namespace eShopOnTelegram.TelegramBot.Constants;

public static class BotContentDefaultConstants
{
    public static class Common
    {
        public const string StartError = "Error. Try again later";
        public const string WelcomeText = "Welcome to our shop";
        public const string OpenShopButtonText = "Click here to open shop";
        public const string UnknownCommandText = "Unknown command was sent";
        public const string DefaultErrorMessage = "Error. Something went wrong. Please try again later";
    }

    public static class Order
    {
        public const string ShowUnpaidOrder = "Show unpaid order";
        public const string NoUnpaidOrderFound = "You don't have unpaid orders";
        public const string AlreadyPaidOrExpired = "You already paid for this order, or time to accomplish expired. Try again to create new order";
        public const string CreateErrorMessage = "Error during order creation";
        public const string InvoiceGenerationFailedErrorMessage = "Error during invoice generation. Try again later";
        public static string OrderNumberTitle(string orderNumber) => $"Order number {orderNumber}";
    }

    public static class Payment
    {
        public const string NoEnabledPayments = "Thank you for your purchase";
        public const string ChoosePaymentMethod = "Please select payment method";
        public const string ProceedToPayment = "Proceed to payment";
        public const string InvoiceReceiveMessage = "Please receive your invoice";
        public const string SuccessfullPayment = "Thank you for purchase";
        public const string ErrorDuringPaymentConfirmation = "Error during order confirmation";
    }
}
