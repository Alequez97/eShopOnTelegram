using System.Text;

using eShopOnTelegram.Domain.Dto.ProductAttributes;

namespace eShopOnTelegram.Domain.Dto.Orders;

public class CartItemDto
{
    public required ProductAttributeDto ProductAttribute { get; set; }

    public required int Quantity { get; set; }

    public decimal TotalPrice => (ProductAttribute.PriceWithDiscount ?? ProductAttribute.OriginalPrice) * Quantity;

    public string GetFormattedMessage(char currencySymbol)
    {
        var ProductAttributeInfoStringBuilder = new StringBuilder();
        ProductAttributeInfoStringBuilder.Append($"{ProductAttribute.ProductName}");

        if (!string.IsNullOrWhiteSpace(ProductAttribute.Color) || !string.IsNullOrWhiteSpace(ProductAttribute.Size))
        {
            ProductAttributeInfoStringBuilder.Append(" (");
            if (!string.IsNullOrWhiteSpace(ProductAttribute.Color))
            {
                ProductAttributeInfoStringBuilder.Append($"{ProductAttribute.Color}");
                if (!string.IsNullOrWhiteSpace(ProductAttribute.Size))
                {
                    ProductAttributeInfoStringBuilder.Append($"{ProductAttribute.Size}");
                }
            }
            else
            {
                ProductAttributeInfoStringBuilder.Append($"{ProductAttribute.Size}");
            }
            ProductAttributeInfoStringBuilder.Append(")");
        }
        ProductAttributeInfoStringBuilder.Append($" (x{Quantity}) {TotalPrice}{currencySymbol}");

        return ProductAttributeInfoStringBuilder.ToString();
    }
}
