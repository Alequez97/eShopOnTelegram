namespace eShopOnTelegram.Domain.Responses.ProductCategories;

public class GetProductCategoryResponse : Response
{
    public long Id { get; set; }

    public string Name { get; set; }
}
