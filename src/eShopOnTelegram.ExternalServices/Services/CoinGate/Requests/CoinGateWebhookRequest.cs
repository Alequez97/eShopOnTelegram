using System.Text.Json.Serialization;

namespace eShopOnTelegram.ExternalServices.Services.CoinGate.Requests;

public class CoinGateWebhookRequest
{
	[JsonPropertyName("order_id")]
	public required string OrderNumber { get; set; }
	
	[JsonPropertyName("status")]
	public required string Status { get; set; }

	[JsonPropertyName("token")]
	public required string CustomValidationToken { get; set; }
}
