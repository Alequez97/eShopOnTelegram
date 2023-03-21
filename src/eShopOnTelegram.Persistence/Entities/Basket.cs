namespace eShopOnTelegram.Persistence.Entities;

public class Basket : EntityBase
{
    public Customer Customer { get; set; }

    public IList<CartItem> CartItems { get; set; }

    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
}
