namespace eShopOnTelegram.Persistence.Entities.Orders;

[Index(nameof(OrderNumber), IsUnique = true)]
public class Order : EntityBase
{
    public required string OrderNumber { get; init; }

    public required long CustomerId { get; init; }
    public Customer Customer { get; set; }

    public required IList<CartItem> CartItems { get; init; }

    public DateTime CreationDate { get; init; } = DateTime.UtcNow;

    public DateTime? PaymentDate { get; set; }

    public required OrderStatus Status { get; set; }
    public required OrderPaymentStatus PaymentStatus { get; set; }
    public required OrderPaymentMethod PaymentMethod { get; set; }


    [StringLength(2, MinimumLength = 2)]
    public string? CountryIso2Code { get; set; }

    [MaxLength(100)]
    public string? City { get; set; }

    [MaxLength(200)]
    public string? StreetLine1 { get; set; }

    [MaxLength(200)]
    public string? StreetLine2 { get; set; }

    [MaxLength(20)]
    public string? PostCode { get; set; }

    public void SetPaymentMethod(OrderPaymentMethod paymentMethod)
    {
        Status = OrderStatus.AwaitingPayment;
        if (paymentMethod == OrderPaymentMethod.Card) PaymentStatus = OrderPaymentStatus.InvoiceSent;
        PaymentMethod = paymentMethod;
    }

    public void ConfirmPayment()
    {
        Status = OrderStatus.Paid;
        PaymentStatus = OrderPaymentStatus.PaymentSuccessful;
    }
}