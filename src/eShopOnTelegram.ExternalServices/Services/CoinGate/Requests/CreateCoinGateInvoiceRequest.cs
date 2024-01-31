
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

	[JsonPropertyName("order_id")]
	public required string OrderNumber { get; set; }

	[JsonPropertyName("callback_url")]
	public required string CallbackUrl { get; set; }

	[JsonPropertyName("token")]
	public required string CustomValidationToken { get; set; }
}
