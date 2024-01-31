using eShopOnTelegram.ExternalServices.Services.CoinGate.Requests;
using eShopOnTelegram.ExternalServices.Services.CoinGate.Responses;

using Refit;

namespace eShopOnTelegram.ExternalServices.Services.CoinGate;

public interface ICoinGateClient
{
	[Post("/orders")]
	public Task<CreateCoinGateInvoiceResponse> CreateInvoiceAsync(
		[Header("Authorization")] string authorizationHeader,
		[Body] CreateCoinGateInvoiceRequest request
	);
}
