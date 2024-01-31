using System.Security.Cryptography;
using System.Text;

using eShopOnTelegram.ExternalServices.Interfaces;
using eShopOnTelegram.ExternalServices.Services.Plisio.Requests;

using Newtonsoft.Json.Linq;

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

		var requestBodyJsonObject = JObject.Parse(requestBody);
		requestBodyJsonObject.Remove("verify_hash");
		var sortedRequestBodyJsonObject = requestBodyJsonObject.Properties().OrderBy(p => p.Name);
		var sortedJsonObject = new JObject(sortedRequestBodyJsonObject.Select(p => new JProperty(p.Name, p.Value)));

		var modifiedRequestBodyToValidate = sortedJsonObject.ToString();

		var calculatedHash = CalculateHMACSHA1(modifiedRequestBodyToValidate, _plisioApiToken);
		var requestReceivedFromPlisio = string.Equals(request.VerifyHash, calculatedHash, StringComparison.OrdinalIgnoreCase);

		return requestReceivedFromPlisio;
	}

	private  string CalculateHMACSHA1(string message, string secretKey)
	{
		byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);
		byte[] messageBytes = Encoding.UTF8.GetBytes(message);

		using var hmac = new HMACSHA1(keyBytes);
		byte[] hashBytes = hmac.ComputeHash(messageBytes);

		return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
	}
}
