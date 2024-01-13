namespace eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Keys;

public static class ApplicationContentKey
{
	public static class TelegramBot
	{
		public const string WelcomeText = "TelegramBot.WelcomeText";
		public const string OpenShopButtonText = "TelegramBot.OpenShopButtonText";
		public const string UnknownCommandText = "TelegramBot.UnknownCommandText";
		public const string DefaultErrorMessage = "TelegramBot.DefaultErrorMessage";
	}

	public static class Order
	{
		public const string ShowUnpaidOrder = "Order.ShowUnpaidOrder";
		public const string AlreadyPaidOrExpired = "Order.AlreadyPaidOrExpired";
		public const string CreateErrorMessage = "Order.CreateErrorMessage";
		public const string InvoiceGenerationFailedErrorMessage = "Order.InvoiceGenerationFailedErrorMessage";
		public const string OrderNumberTitle = "Order.OrderNumberTitle";
		public const string OrderSummaryTitle = "Order.OrderSummaryTitle";
		public const string TotalPriceTitle = "Order.TotalPriceTitle";
		public const string PaymentMethodAlreadySelected = "Order.PaymentMethodAlreadySelected";
		public const string UnableToGetShippingAddress = "Order.UnableToGetShippingAddress";
	}

	public static class Payment
	{
		public const string NoEnabledPayments = "Payment.NoEnabledPayments";
		public const string ChoosePaymentMethod = "Payment.ChoosePaymentMethod";
		public const string PayWithBankCard = "Payment.PayWithBankCard";
		public const string DiscussPaymentOptionsWithSeller = "Payment.DiscussPaymentOptionsWithSeller";
		public const string InvoiceReceiveMessage = "Payment.InvoiceReceiveMessage";
		public const string IncorrectInvoiceChoosen = "Payment.IncorrectInvoiceChoosen";
		public const string SuccessfullPayment = "Payment.SuccessfullPayment";
		public const string ErrorDuringPaymentConfirmation = "Payment.ErrorDuringPaymentConfirmation";
		public const string PaymentThroughSeller = "Payment.PaymentThroughSeller";
	}
}
