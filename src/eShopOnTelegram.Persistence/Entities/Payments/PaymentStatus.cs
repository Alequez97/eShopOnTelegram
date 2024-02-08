namespace eShopOnTelegram.Persistence.Entities.Payments;

public enum PaymentStatus
{
	// ? Refunded
	// ? PaymentFailure
	None = 0,
	InvoiceSent = 1,
	PaymentSuccessful = 2,
}
