namespace eShopOnTelegram.Persistence.Entities.Products;

public class ProductAttribute : EntityBase
{
	public string? Color { get; set; }

	public string? Size { get; set; }

	public required decimal OriginalPrice { get; set; }

	public decimal? PriceWithDiscount { get; set; }

	public required int QuantityLeft { get; set; }

	[MaxLength(200)]
	public required string ImageName { get; set; }

	public long ProductId { get; set; }
	public Product Product { get; set; }

	public required bool IsDeleted { get; set; }


	public long? PreviousVersionId { get; set; }
	public ProductAttribute? PreviousVersion { get; set; }

	public void DecreaseQuantity(int amountToDecrease)
	{
		QuantityLeft -= amountToDecrease;
	}

	public string ProductName => Product.Name;

	public string ProductCategoryName => Product.Category.Name;
}
