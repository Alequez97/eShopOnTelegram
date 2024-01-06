namespace eShopOnTelegram.Domain.Requests.Orders;

public class CreateCartItemRequest
{
	public required long ProductAttributeId { get; set; }

	public required int Quantity { get; set; }
}
