namespace eShopOnTelegram.Persistence.Entities;

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
