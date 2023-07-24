using System.Security.Cryptography;
using System.Text;

using eShopOnTelegram.ExternalServices.Interfaces;
using eShopOnTelegram.ExternalServices.Services.Plisio.Requests;

using Newtonsoft.Json;

namespace eShopOnTelegram.ExternalServices.Services.Plisio.Validators;

public class PlisioPaymentReceivedWebhookRequestValidator : IWebhookRequestValidator<PlisioPaymentReceivedWebhookRequest>
{
    private readonly string _plisioApiToken;

    public PlisioPaymentReceivedWebhookRequestValidator(PaymentAppsettings configuration)
    {
        _plisioApiToken = configuration["Payment:Plisio:ApiToken"];
    }

    public bool Validate(PlisioPaymentReceivedWebhookRequest request)
    {
        var serializedRequest = JsonConvert.SerializeObject(request);

        var calculatedHash = HMAC_SHA1(Encoding.UTF8.GetBytes(_plisioApiToken), Encoding.UTF8.GetBytes(serializedRequest));
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
