using eShopOnTelegram.Persistence.Entities.Orders;

namespace eShopOnTelegram.Persistence.Entities.Payments;

public class Payment : EntityBase
{
	public long OrderId { get; set; }
	public Order Order { get; set; }

	public required PaymentStatus PaymentStatus { get; set; }

	public required PaymentMethod PaymentMethod { get; set; }

	public string? InvoiceUrl { get; set; }

	public string? PaymentValidationToken { get; set; }

	public DateTime? PaymentDate { get; set; }

	public void SetPaymentMethod(PaymentMethod paymentMethod)
	{
		PaymentStatus = PaymentStatus.InvoiceSent;
		PaymentMethod = paymentMethod;
	}

	public void ConfirmPayment()
	{
		PaymentStatus = PaymentStatus.PaymentSuccessful;
		PaymentDate = DateTime.UtcNow;
	}
}
