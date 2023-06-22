namespace eShopOnTelegram.ApplicationContent.Keys;

public static class ApplicationContentKey
{
    public static class Common
    {
        public const string StartError = "Common:StartError";
        public const string WelcomeText = "Common:WelcomeText";
        public const string OpenShopButtonText = "Common:OpenShopButtonText";
        public const string UnknownCommandText = "Common:UnknownCommandText";
        public const string DefaultErrorMessage = "Common:DefaultErrorMessage";
    }

    public static class Order
    {
        public const string ShowUnpaidOrder = "Order:ShowUnpaidOrder";
        public const string NoUnpaidOrderFound = "Order:NoUnpaidOrderFound";
        public const string AlreadyPaidOrExpired = "Order:AlreadyPaidOrExpired";
        public const string CreateErrorMessage = "Order:CreateErrorMessage";
        public const string InvoiceGenerationFailedErrorMessage = "Order:InvoiceGenerationFailedErrorMessage";
        public const string OrderNumberTitle = "Order:OrderNumberTitle";
        public const string OrderSummaryTitle = "Order:OrderSummaryTitle";
        public const string TotalPriceTitle = "Order:TotalPriceTitle";
    }

    public static class Payment
    {
        public const string NoEnabledPayments = "Payment:NoEnabledPayments";
        public const string ChoosePaymentMethod = "Payment:ChoosePaymentMethod";
        public const string ProceedToPayment = "Payment:ProceedToPayment";
        public const string InvoiceReceiveMessage = "Payment:InvoiceReceiveMessage";
        public const string SuccessfullPayment = "Payment:SuccessfullPayment";
        public const string ErrorDuringPaymentConfirmation = "Payment:ErrorDuringPaymentConfirmation";
    }
}