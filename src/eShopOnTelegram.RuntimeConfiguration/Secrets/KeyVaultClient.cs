using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

using eShopOnTelegram.RuntimeConfiguration.Secrets.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.Secrets.Requests;
using eShopOnTelegram.Utils.Configuration;

namespace eShopOnTelegram.RuntimeConfiguration.Secrets;
public class KeyVaultClient : IKeyVaultClient
{
	private readonly SecretClient _secretClient;
	private readonly ISecretsNameMapper _secretsNameMapper;

	public KeyVaultClient(AppSettings appSettings, ISecretsNameMapper secretsNameMapper)
	{
		var azureAppSettings = appSettings.AzureSettings;
		var azureCredentials = new ClientSecretCredential(azureAppSettings.TenantId, azureAppSettings.ClientId, azureAppSettings.ClientSecret);

		_secretClient = new SecretClient(new Uri(appSettings.AzureSettings.KeyVaultUri), azureCredentials);
		_secretsNameMapper = secretsNameMapper;
	}

	public async Task CreateOrUpdateAsync(CreateOrUpdateSecretRequest request)
	{
		var secretPrivateName = _secretsNameMapper.GetPrivateSecretName(request.PublicSecretName);

		if (secretPrivateName == null)
		{
			throw new ArgumentException($"Tried to create or update key, by public name, that is not mapped to any {nameof(request.PublicSecretName)}");
		}

		await _secretClient.SetSecretAsync(secretPrivateName, request.Value);
	}
}
