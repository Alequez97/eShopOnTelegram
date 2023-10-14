namespace eShopOnTelegram.Domain.Requests.ProductAttributes;

public class UpdateProductAttributeRequest
{
    public required long Id { get; set; }

    public string? Color { get; set; }

    public string? Size { get; set; }

    public required decimal OriginalPrice { get; set; }

    public decimal? PriceWithDiscount { get; set; }

    public required int QuantityLeft { get; set; }

    public byte[]? ImageAsBase64 { get; set; }

    public string? ImageName { get; set; }
}
