namespace eShopOnTelegram.Domain.Dto.ProductAttributes;

public class ProductAttributeDto
{
    public required long Id { get; set; }

    // TODO: Remove required size and color, this is to ensure fields are mapped in all places
    public required string Color { get; set; }

    public required string Size { get; set; }

    public required string ProductName { get; set; }

    public required string ProductCategoryName { get; set; }

    public required decimal OriginalPrice { get; set; }

    public decimal? PriceWithDiscount { get; set; }

    public required int QuantityLeft { get; set; }

    public string? Image { get; set; }
}
