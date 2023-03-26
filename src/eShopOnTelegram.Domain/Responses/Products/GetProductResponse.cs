namespace eShopOnTelegram.Domain.Responses.Products;

public class GetProductResponse : Response
{
    public string ProductName { get; set; }

    public string ProductCategoryName { get; set; }

    public double OriginalPrice { get; set; }

    public double? PriceWithDiscount { get; set; }

    public int QuantityLeft { get; set; }

    public string Image { get; set; }
}
