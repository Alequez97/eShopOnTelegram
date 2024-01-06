namespace eShopOnTelegram.Persistence.Entities.Products;

[Index(nameof(Name), IsUnique = true)]
public class ProductCategory : EntityBase
{
	[MaxLength(100)]
	public required string Name { get; set; }

	public required bool IsDeleted { get; set; }

	public ProductCategory? PreviousVersion { get; set; }
}
