using eShopOnTelegram.ApplicationContent.Models;

namespace eShopOnTelegram.ApplicationContent.Interfaces;

public interface IApplicationContentStore
{
    Task<ApplicationContentModel> GetApplicationContentAsync(CancellationToken cancellationToken);

    Task<string> GetValueAsync(string key, CancellationToken cancellationToken);

    Task<bool> UpdateContentAsync(Dictionary<string, string> keyValues, CancellationToken cancellationToken);
}
