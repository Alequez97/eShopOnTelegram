using eShopOnTelegram.RuntimeConfiguration.Secrets.Constants;
using eShopOnTelegram.RuntimeConfiguration.Secrets.Interfaces;

namespace eShopOnTelegram.RuntimeConfiguration.Secrets;

public class SecretsNameMapper : ISecretsNameMapper
{
	private readonly SecretsMappingConfig _secretsMappingConfig;

	public SecretsNameMapper(
		SecretsMappingConfig secretsMappingConfig
		)
	{
		_secretsMappingConfig = secretsMappingConfig;
	}

	public string? GetPrivateSecretName(string publicSecretName)
	{
		var privateConfigItem = _secretsMappingConfig.PrivateConfig.FirstOrDefault(configItem => configItem.PublicSecretName == publicSecretName);
		return privateConfigItem?.PrivateSecretName;
	}

	public string? GetPublicSecretName(string privateSecretName)
	{
		var publicConfigItem = _secretsMappingConfig.PrivateConfig.FirstOrDefault(configItem => configItem.PrivateSecretName == privateSecretName);
		return publicConfigItem?.PublicSecretName;
	}
}
