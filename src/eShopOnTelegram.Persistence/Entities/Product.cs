namespace eShopOnTelegram.Persistence.Entities;

[Index(nameof(Name), IsUnique = true)]
public class Product : EntityBase
{
    public required string Name { get; set; }

    public ProductCategory Category { get; set; }

    public required decimal OriginalPrice { get; set; }

    public decimal? PriceWithDiscount { get; set; }

    public required int QuantityLeft { get; set; }

    [MaxLength(200)]
    public string? ImageName { get; set; }
}
