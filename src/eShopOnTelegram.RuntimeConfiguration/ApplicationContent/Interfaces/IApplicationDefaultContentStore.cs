using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Content;

namespace eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;

public interface IApplicationDefaultContentStore
{
    Task<ApplicationContentModel> GetDefaultApplicationContentAsync(CancellationToken cancellationToken);

    Task<string> GetDefaultValueAsync(string key, CancellationToken cancellationToken);
}
