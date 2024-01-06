namespace eShopOnTelegram.RuntimeConfiguration.Secrets.Interfaces;

public interface ISecretsNameMapper
{
    string? GetPublicSecretName(string privateSecretName);

    string? GetPrivateSecretName(string publicSecretName);
}
