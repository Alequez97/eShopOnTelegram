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
        public const string CreateOrderErrorMessage = "Error during order creation";
    }

    public static class Payment
    {
        public const string NoEnabledPayments = "Thank you for your purchase";
        public const string ChoosePaymentMethod = "Please select payment method";
        public const string SuccessfullPayment = "Thank you for purchase";
        public const string ErrorDuringPaymentConfirmation = "Error during order confirmation";
    }
}
