using eShopOnTelegram.Domain.Requests.ProductAttributes;

namespace eShopOnTelegram.Domain.Requests.Products;

public class UpdateProductRequest
{
    public long Id { get; set; }

    public string Name { get; set; }

    public List<UpdateProductAttributeRequest> ProductAttributes { get; set; }
}