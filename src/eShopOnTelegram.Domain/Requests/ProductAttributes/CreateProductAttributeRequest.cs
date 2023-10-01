using eShopOnTelegram.Domain.Validation.Attributes;
using Microsoft.AspNetCore.Http;

namespace eShopOnTelegram.Domain.Requests.ProductAttributes;

public class CreateProductAttributeRequest
{
    public required string Color { get; set; }

    public required string Size { get; set; }

    public required decimal OriginalPrice { get; set; }

    public decimal? PriceWithDiscount { get; set; }

    public required int QuantityLeft { get; set; }

    [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".gif" })]
    public required IFormFile ProductImage { get; set; }
}
