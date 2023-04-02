namespace eShopOnTelegram.Domain.Requests.Orders
{
    public class CreateOrderRequest
    {
        public required long TelegramUserUID { get; set; }
        public required IList<CartItem> CartItems { get; set; }
    }
}
