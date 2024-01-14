namespace eShopOnTelegram.Translations.Constants;

public static class TranslationsKeys
{
	// Values, that can be overriden later by shop administrator
	public const string WelcomeToOurShop = nameof(WelcomeToOurShop);
	public const string UnknownCommand = nameof(UnknownCommand);
	public const string DefaultErrorMessage = nameof(DefaultErrorMessage);
	public const string ThankYouForPurchase = nameof(ThankYouForPurchase);
	public const string PaymentThroughSeller = nameof(PaymentThroughSeller);

	// Constants, that can't be changed by shop administrator
	public const string Error_TryAgainLater = nameof(Error_TryAgainLater);
	public const string NoAvailableProducts = nameof(NoAvailableProducts);
	public const string AllCategories = nameof(AllCategories);
	public const string Continue = nameof(Continue);
	public const string ProceedToPayment = nameof(ProceedToPayment);
	public const string UnpaidOrderNotFound = nameof(UnpaidOrderNotFound);
	public const string ErrorDuringOrderCreation = nameof(ErrorDuringOrderCreation);
	public const string IncorrectInvoiceChoosen = nameof(IncorrectInvoiceChoosen);
	public const string OrderNumber = nameof(OrderNumber);
	public const string OrderSummary = nameof(OrderSummary);
	public const string UnableToGetShippingAddress = nameof(UnableToGetShippingAddress);
	public const string ChoosePaymentMethod = nameof(ChoosePaymentMethod);
	public const string PayWithBankCard = nameof(PayWithBankCard);
	public const string OrderAlreadyPaidOrExpired = nameof(OrderAlreadyPaidOrExpired);
	public const string InvoiceGenerationFailedErrorMessage = nameof(InvoiceGenerationFailedErrorMessage);
	public const string TotalPrice = nameof(TotalPrice);
	public const string OpenShop = nameof(OpenShop);
	public const string ShowUnpaidOrder = nameof(ShowUnpaidOrder);
	public const string PaymentMethodAlreadySelected = nameof(PaymentMethodAlreadySelected);
	public const string NoEnabledPaymentMethods = nameof(NoEnabledPaymentMethods);
	public const string InvoiceReceived = nameof(InvoiceReceived);
}
