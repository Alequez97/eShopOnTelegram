using eShopOnTelegram.ExternalServices.Interfaces;
using eShopOnTelegram.ExternalServices.Services.CoinGate.Requests;

namespace eShopOnTelegram.ExternalServices.Services.CoinGate.Validators;

public class CoinGateWebhookValidator : IWebhookValidator<CoinGateWebhookRequest>
{
	private readonly string _coinGateApiToken;

	public CoinGateWebhookValidator(string coinGateApiToken)
	{
		_coinGateApiToken = coinGateApiToken;
	}

	public bool Validate(CoinGateWebhookRequest request, string requestBody)
	{
		return true;
	}
}
