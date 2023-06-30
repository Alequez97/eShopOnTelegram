using eShopOnTelegram.Domain.Validation.Attributes;

using Microsoft.AspNetCore.Http;

namespace eShopOnTelegram.Domain.Requests.Products;

public class CreateProductRequest
{
    public required string Name { get; set; }

    public required long ProductCategoryId { get; set; }

    public required decimal OriginalPrice { get; set; }

    public decimal? PriceWithDiscount { get; set; }

    public required int QuantityLeft { get; set; }

    [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".gif" })]
    public required IFormFile ProductImage { get; set; }
}
