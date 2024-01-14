namespace eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Keys;

public static class ApplicationContentKey
{
	public static class TelegramBot
	{
		public const string WelcomeText = "TelegramBot.WelcomeText";
		public const string UnknownCommandText = "TelegramBot.UnknownCommandText";
		public const string DefaultErrorMessage = "TelegramBot.DefaultErrorMessage";
	}

	public static class Payment
	{
		public const string SuccessfullPayment = "Payment.SuccessfullPayment";
		public const string PaymentThroughSeller = "Payment.PaymentThroughSeller";
	}
}
