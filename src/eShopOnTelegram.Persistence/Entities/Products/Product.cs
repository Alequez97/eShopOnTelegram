namespace eShopOnTelegram.Persistence.Entities.Products;

[Index(nameof(Name), IsUnique = false)]
public class Product : EntityBase
{
	public required string Name { get; set; }

	public long CategoryId { get; set; }
	public ProductCategory Category { get; set; }

	public List<ProductAttribute> ProductAttributes { get; set; }

	public required bool IsDeleted { get; set; }

	public Product? PreviousVersion { get; set; }
}
