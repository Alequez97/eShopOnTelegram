namespace eShopOnTelegram.Persistence.Entities;

[Index(nameof(Name), IsUnique = true)]
public class ProductCategory : EntityBase
{
    [MaxLength(100)]
    public required string Name { get; set; }
}
