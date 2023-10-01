using eShopOnTelegram.Domain.Dto.ProductAttributes;

namespace eShopOnTelegram.Domain.Dto.Orders;

public class CartItemDto
{
    public required ProductAttributeDto ProductAttribute { get; set; }

    public required int Quantity { get; set; }

    public decimal TotalPrice => ProductAttribute.TotalPrice * Quantity;
}
