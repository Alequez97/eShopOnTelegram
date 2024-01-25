using System.Text.Json.Serialization;

namespace eShopOnTelegram.ExternalServices.Services.Plisio.Requests;

public class PlisioPaymentReceivedWebhookRequest
{
	[JsonPropertyName("txn_id")]
	public required string TransactionId { get; set; }

	[JsonPropertyName("order_number")]
	public required string OrderNumber { get; set; }

	[JsonPropertyName("status")]
	public required string Status { get; set; }

	[JsonPropertyName("verify_hash")]
	public required string VerifyHash { get; set; }
}
