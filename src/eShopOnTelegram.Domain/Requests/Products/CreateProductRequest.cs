using eShopOnTelegram.Domain.Requests.ProductAttributes;

namespace eShopOnTelegram.Domain.Requests.Products;

public class CreateProductRequest
{
    public required string Name { get; set; }

    public string? SecondName { get; set; }

    public required long ProductCategoryId { get; set; }

    public required List<CreateProductAttributeRequest> ProductAttributes { get; set; }
}
