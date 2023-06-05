using eShopOnTelegram.ExternalServices.Services.Plisio.Responses;

using Refit;

namespace eShopOnTelegram.ExternalServices.Services.Plisio;

public interface IPlicioClient
{
    [Get("/invoices/new?source_currency={sourceCurrency}&source_amount={sourceAmount}&order_number={orderNumber}&currency={currency}&api_key={apiKey}")]
    public Task<CreatePlicioInvoiceResponse> CreateInvoiceAsync(string apiKey, string sourceCurrency, int sourceAmount, string orderNumber, string currency, string order_name = "btc1");
}
