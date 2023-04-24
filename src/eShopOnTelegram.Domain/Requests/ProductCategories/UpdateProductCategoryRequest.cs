namespace eShopOnTelegram.Domain.Requests.ProductCategories;

public class UpdateProductCategoryRequest
{
    public required long Id { get; set; }

    public required string Name { get; set; }
}
