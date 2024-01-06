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
		var productAttributeInfoStringBuilder = new StringBuilder();
		productAttributeInfoStringBuilder.Append($"{ProductAttribute.ProductName}");

		if (!string.IsNullOrWhiteSpace(ProductAttribute.Color) || !string.IsNullOrWhiteSpace(ProductAttribute.Size))
		{
			productAttributeInfoStringBuilder.Append(" (");
			if (!string.IsNullOrWhiteSpace(ProductAttribute.Color))
			{
				productAttributeInfoStringBuilder.Append($"{ProductAttribute.Color}");
				if (!string.IsNullOrWhiteSpace(ProductAttribute.Size))
				{
					productAttributeInfoStringBuilder.Append($", ");
					productAttributeInfoStringBuilder.Append($"{ProductAttribute.Size}");
				}
			}
			else
			{
				productAttributeInfoStringBuilder.Append($"{ProductAttribute.Size}");
			}
			productAttributeInfoStringBuilder.Append(")");
		}

		productAttributeInfoStringBuilder.Append($" (x{Quantity}) {TotalPrice}{currencySymbol}");

		return productAttributeInfoStringBuilder.ToString();
	}
}
