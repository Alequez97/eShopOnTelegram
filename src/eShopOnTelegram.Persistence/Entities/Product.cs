namespace eShopOnTelegram.Persistence.Entities;

public class Product : EntityBase
{
    public string Name { get; set; }

    public ProductCategory Category { get; set; }

    public double OriginalPrice { get; set; }

    public double? PriceWithDiscount { get; set; }

    public int QuantityLeft { get; set; }

    [MaxLength(200)]
    public string ImageName { get; set; }
}
