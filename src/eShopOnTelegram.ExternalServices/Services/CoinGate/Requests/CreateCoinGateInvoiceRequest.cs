
using System.Text.Json.Serialization;

namespace eShopOnTelegram.ExternalServices.Services.CoinGate.Requests;

public class CreateCoinGateInvoiceRequest
{
	[JsonPropertyName("price_amount")]
	public required int PriceAmount { get; set; }

	[JsonPropertyName("price_currency")]
	public required string PriceCurrency { get; set; }

	[JsonPropertyName("receive_currency")]
	public required string ReceiveCurrency { get; set; }
}
