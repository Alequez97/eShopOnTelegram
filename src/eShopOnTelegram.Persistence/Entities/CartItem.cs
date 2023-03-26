namespace eShopOnTelegram.Persistence.Entities;

public class CartItem : EntityBase
{
    public required long ProductId { get; set; }
    public Product Product { get; set; }

    public required int Quantity { get; set; }
}