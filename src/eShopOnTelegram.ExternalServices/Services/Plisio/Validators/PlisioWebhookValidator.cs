using System.Security.Cryptography;
using System.Text;

using eShopOnTelegram.ExternalServices.Interfaces;
using eShopOnTelegram.ExternalServices.Services.Plisio.Requests;

namespace eShopOnTelegram.ExternalServices.Services.Plisio.Validators;

public class PlisioWebhookValidator : IWebhookValidator<PlisioWebhookRequest>
{
	private readonly string _plisioApiToken;

	public PlisioWebhookValidator(string plisioApiToken)
	{
		_plisioApiToken = plisioApiToken;
	}

	public bool Validate(PlisioWebhookRequest request, string requestBody)
	{
		if (string.IsNullOrWhiteSpace(request.VerifyHash))
		{
			return false;
		}

		var calculatedHash = HMAC_SHA1(Encoding.UTF8.GetBytes(_plisioApiToken), Encoding.UTF8.GetBytes(requestBody));
		var calculatedHashHex = BitConverter.ToString(calculatedHash).Replace("-", string.Empty);
		var requestReceivedFromPlisio = string.Equals(request.VerifyHash, calculatedHashHex, StringComparison.OrdinalIgnoreCase);

		return requestReceivedFromPlisio;
	}

	private static byte[] HMAC_SHA1(byte[] key, byte[] data)
	{
		using HMACSHA1 hmac = new HMACSHA1(key);
		return hmac.ComputeHash(data);
	}
}
