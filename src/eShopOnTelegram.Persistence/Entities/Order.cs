namespace eShopOnTelegram.Persistence.Entities;

[Index(nameof(OrderNumber), IsUnique = true)]
public class Order : EntityBase
{
    public required string OrderNumber { get; set; }

    public required long CustomerId { get; set; }
    public Customer Customer { get; set; }

    public required IList<CartItem> CartItems { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? PaymentDate { get; set; }

    public required OrderStatus Status { get; set; }

    [MaxLength(2)]
    public string? CountryIso2Code { get; set; }

    [MaxLength(100)]
    public string? City { get; set; }

    [MaxLength(200)]
    public string? StreetLine1 { get; set; }

    [MaxLength(200)]
    public string? StreetLine2 { get; set; }

    [MaxLength(20)]
    public string? PostCode { get; set; }
}