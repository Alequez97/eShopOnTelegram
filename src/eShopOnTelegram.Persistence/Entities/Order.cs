using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopOnTelegram.Persistence.Entities;

[Index(nameof(OrderNumber), IsUnique = true)]
public class Order : EntityBase
{
    public string OrderNumber { get; set; }

    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }

    public IList<CartItem> CartItems { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? PaymentDate { get; set; }

    public OrderStatus Status { get; set; }

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