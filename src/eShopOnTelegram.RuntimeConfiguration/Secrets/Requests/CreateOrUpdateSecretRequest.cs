namespace eShopOnTelegram.RuntimeConfiguration.Secrets.Requests;

public class CreateOrUpdateSecretRequest
{
    public required string PublicSecretName { get; set; }
    public required string Value { get; set; }
}
