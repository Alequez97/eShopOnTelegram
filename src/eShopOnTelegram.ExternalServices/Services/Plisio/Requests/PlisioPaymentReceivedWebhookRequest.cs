using Newtonsoft.Json;

namespace eShopOnTelegram.ExternalServices.Services.Plisio.Requests;

public class PlisioPaymentReceivedWebhookRequest
{
    [JsonProperty("txn_id")]
    public string TransactionId { get; set; }

    [JsonProperty("order_number")]
    public string OrderNumber { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("verify_hash")]
    public string VerifyHash { get; set; }
}
