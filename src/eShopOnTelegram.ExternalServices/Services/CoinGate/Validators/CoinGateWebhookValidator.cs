using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.ExternalServices.Interfaces;
using eShopOnTelegram.ExternalServices.Services.CoinGate.Requests;

namespace eShopOnTelegram.ExternalServices.Services.CoinGate.Validators;

public class CoinGateWebhookValidator : IWebhookValidator<CoinGateWebhookRequest>
{
	private readonly IPaymentService _paymentService;

	public CoinGateWebhookValidator(IPaymentService paymentService)
	{
		_paymentService = paymentService;
	}

	public async Task<bool> ValidateAsync(CoinGateWebhookRequest request, string requestBody = null, CancellationToken cancellationToken = default)
	{
		var getValidationTokenResponse = await _paymentService.GetValidationTokenAsync(request.OrderNumber, cancellationToken);

		if (getValidationTokenResponse.Status != ResponseStatus.Success)
		{
			return false;
		}

		return request.CustomValidationToken == getValidationTokenResponse.Data;
	}
}
