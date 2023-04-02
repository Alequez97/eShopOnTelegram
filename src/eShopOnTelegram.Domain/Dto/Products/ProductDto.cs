namespace eShopOnTelegram.Domain.Dto.Products; 

public class ProductDto : DtoBase
{
    public required string ProductName { get; set; }

    public required string ProductCategoryName { get; set; }

    public required decimal OriginalPrice { get; set; }

    public decimal? PriceWithDiscount { get; set; }

    public required int QuantityLeft { get; set; }

    public string? Image { get; set; }
}
