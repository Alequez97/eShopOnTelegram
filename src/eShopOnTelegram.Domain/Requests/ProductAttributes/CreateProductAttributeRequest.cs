namespace eShopOnTelegram.Domain.Requests.ProductAttributes;

public class CreateProductAttributeRequest
{
	public string? Color { get; set; }

	public string? Size { get; set; }

	public required decimal OriginalPrice { get; set; }

	public decimal? PriceWithDiscount { get; set; }

	public required int QuantityLeft { get; set; }

	public required byte[] ImageAsBase64 { get; set; }

	public required string ImageName { get; set; }
}
