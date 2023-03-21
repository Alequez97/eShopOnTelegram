namespace eShopOnTelegram.Persistence.Entities;

public class CartItem : EntityBase
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; }

    public string ProductNameDuringOrderCreation { get; set; }

    public int Quantity { get; set; }
}