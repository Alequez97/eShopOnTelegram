using System.Text.Json.Serialization;

namespace eShopOnTelegram.ExternalServices.Services.Plisio.Responses;

public class CreatePlicioInvoiceResponse
{
    public string Status { get; set; }

    [JsonPropertyName("data")]
    public Data Data { get; set; }
}

public class Data
{
    [JsonPropertyName("invoice_url")]
    public string InvoiceUrl { get; set; }

    [JsonPropertyName("txn_id")]
    public string TransactionId { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }
}
