namespace eShopOnTelegram.ExternalServices.Services.CoinGate.Requests;

public class CoinGateWebhookRequest
{
	public required string OrderNumber { get; set; }
}
