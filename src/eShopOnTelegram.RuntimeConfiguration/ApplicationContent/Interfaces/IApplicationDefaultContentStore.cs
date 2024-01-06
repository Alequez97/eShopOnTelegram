namespace eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;

public interface IApplicationDefaultContentStore
{
	Task<string> GetDefaultApplicationContentAsJsonStringAsync(CancellationToken cancellationToken);

	Task<string> GetDefaultValueAsync(string key, CancellationToken cancellationToken);
}
