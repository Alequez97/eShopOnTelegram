using eShopOnTelegram.ApplicationContent.Models;

namespace eShopOnTelegram.ApplicationContent.Interfaces;

public interface IApplicationDefaultContentStore
{
    Task<ApplicationContentModel> GetDefaultApplicationContentAsync(CancellationToken cancellationToken);

    Task<string> GetDefaultValueAsync(string key, CancellationToken cancellationToken);
}
