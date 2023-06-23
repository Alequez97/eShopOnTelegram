using eShopOnTelegram.ApplicationContent.Models;

namespace eShopOnTelegram.ApplicationContent.Interfaces;

public interface IApplicationContentStore
{
    Task<ApplicationContentModel> GetApplicationContentAsync(CancellationToken cancellationToken);

    Task<string> GetSingleValueAsync(string key, CancellationToken cancellationToken);

    Task UpdateContentAsync(List<KeyValuePair<string, string>> keyValues, CancellationToken cancellationToken);
}
