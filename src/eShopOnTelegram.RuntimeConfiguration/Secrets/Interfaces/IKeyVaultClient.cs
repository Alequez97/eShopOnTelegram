using eShopOnTelegram.RuntimeConfiguration.Secrets.Requests;

namespace eShopOnTelegram.RuntimeConfiguration.Secrets.Interfaces;

public interface IKeyVaultClient
{
	Task CreateOrUpdateAsync(CreateOrUpdateSecretRequest request);
}
