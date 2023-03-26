namespace eShopOnTelegram.Domain.Responses.Products;

public class GetProductResponse
{
    public long Id { get; set; }

    public string ProductName { get; set; }

    public string ProductCategoryName { get; set; }

    public double OriginalPrice { get; set; }

    public double? PriceWithDiscount { get; set; }

    public int QuantityLeft { get; set; }

    public string Image { get; set; }
}
