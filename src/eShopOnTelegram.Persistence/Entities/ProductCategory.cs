namespace eShopOnTelegram.Persistence.Entities;

public class ProductCategory : EntityBase
{
    [MaxLength(100)]
    public required string Name { get; set; }
}
