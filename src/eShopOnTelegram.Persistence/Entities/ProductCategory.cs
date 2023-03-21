namespace eShopOnTelegram.Persistence.Entities;

public class ProductCategory : EntityBase
{
    [MaxLength(100)]
    public string Name { get; set; }
}
