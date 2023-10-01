namespace eShopOnTelegram.Persistence.Entities;

public class ProductAttribute : EntityBase
{
    // TODO: Remove required, this is to ensure field is mapped in all places
    public required string Color { get; set; }

    public required string Size { get; set; }

    public required decimal OriginalPrice { get; set; }

    public decimal? PriceWithDiscount { get; set; }

    public required int QuantityLeft { get; set; }

    [MaxLength(200)]
    public required string ImageName { get; set; }

    public long ProductId { get; set; }
    public Product Product { get; set; }

    public required bool IsDeleted { get; set; }

    public ProductAttribute? PreviousVersion { get; set; }

    public void DecreaseQuantity(int amountToDecrease)
    {
        this.QuantityLeft -= amountToDecrease;
    }

    public string ProductName => Product.Name;

    public string ProductCategoryName => Product.Category.Name;
}