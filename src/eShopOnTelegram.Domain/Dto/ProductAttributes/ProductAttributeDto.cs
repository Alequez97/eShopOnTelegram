namespace eShopOnTelegram.Domain.Dto.ProductAttributes;

public class ProductAttributeDto
{
    // TODO: Remove required, this is to ensure field is mapped in all places
    public required string Color { get; set; }

    public required string Size { get; set; }

    public required string ProductName { get; set; }

    public required string ProductCategoryName { get; set; }

    public required decimal OriginalPrice { get; set; }

    public decimal? PriceWithDiscount { get; set; }

    public required int QuantityLeft { get; set; }

    public string? Image { get; set; }

    public decimal TotalPrice => (PriceWithDiscount ?? OriginalPrice) * QuantityLeft;

}
