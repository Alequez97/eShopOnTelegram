using eShopOnTelegram.Domain.Validation.Attributes;

using Microsoft.AspNetCore.Http;

namespace eShopOnTelegram.Domain.Requests.Products;

public class CreateProductRequest
{
    public string ProductName { get; set; }

    public long ProductCategoryId { get; set; }

    public decimal OriginalPrice { get; set; }

    public decimal? PriceWithDiscount { get; set; }

    public int QuantityLeft { get; set; }

    [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".gif" })]
    public IFormFile ProductImage { get; set; }
}
