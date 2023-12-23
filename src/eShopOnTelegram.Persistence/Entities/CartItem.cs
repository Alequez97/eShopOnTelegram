using eShopOnTelegram.Persistence.Entities.Products;

namespace eShopOnTelegram.Persistence.Entities;

public class CartItem : EntityBase
{
    public required long ProductAttributeId { get; set; }
    public ProductAttribute ProductAttribute { get; set; }

    public required int Quantity { get; set; }
}