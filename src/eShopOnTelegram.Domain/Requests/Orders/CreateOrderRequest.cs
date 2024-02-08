namespace eShopOnTelegram.Domain.Requests.Orders;

public class CreateOrderRequest
{
	public required long TelegramUserUID { get; set; }

	public required IList<CreateCartItemRequest> CartItems { get; set; }

	public string? Country { get; set; }

	public string? City { get; set; }

	public string? StreetLine1 { get; set; }

	public string? StreetLine2 { get; set; }

	public string? PostCode { get; set; }
}
