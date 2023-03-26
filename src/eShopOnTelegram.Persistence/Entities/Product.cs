namespace eShopOnTelegram.Persistence.Entities;

public class Product : EntityBase
{
    public string Name { get; set; }

    public ProductCategory Category { get; set; }

    public decimal OriginalPrice { get; set; }

    public decimal? PriceWithDiscount { get; set; }

    public int QuantityLeft { get; set; }

    [MaxLength(200)]
    public string ImageName { get; set; }
}
