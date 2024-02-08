using eShopOnTelegram.Persistence.Entities.Payments;

namespace eShopOnTelegram.Persistence.Entities.Orders;

[Index(nameof(OrderNumber), IsUnique = true)]
public class Order : EntityBase
{
	public required string OrderNumber { get; init; }

	public required long CustomerId { get; init; }
	public Customer Customer { get; set; }

	public required IList<CartItem> CartItems { get; init; }

	public DateTime CreationDate { get; init; } = DateTime.UtcNow;

	public required OrderStatus Status { get; set; }

	public required Payment PaymentDetails { get; set; }

	[MaxLength(100)]
	public string? Country { get; set; }

	[MaxLength(100)]
	public string? City { get; set; }

	[MaxLength(200)]
	public string? StreetLine1 { get; set; }

	[MaxLength(200)]
	public string? StreetLine2 { get; set; }

	[MaxLength(20)]
	public string? PostCode { get; set; }

	public void SetPaymentMethod(PaymentMethod paymentMethod)
	{
		Status = OrderStatus.AwaitingPayment;
		PaymentDetails.SetPaymentMethod(paymentMethod);
	}

	public void ConfirmPayment()
	{
		Status = OrderStatus.Paid;
		PaymentDetails.ConfirmPayment();
	}
}
