using System.Text.Json.Serialization;

namespace eShopOnTelegram.ExternalServices.Services.CoinGate.Responses;

public class CreateCoinGateInvoiceResponse
{
	[JsonPropertyName("payment_url")]
	public required string PaymentUrl { get; set; }
}
