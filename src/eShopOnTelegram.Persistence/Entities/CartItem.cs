namespace eShopOnTelegram.Persistence.Entities;

public class CartItem : EntityBase
{
    public long ProductId { get; set; }
    public Product Product { get; set; }

    public int Quantity { get; set; }
}