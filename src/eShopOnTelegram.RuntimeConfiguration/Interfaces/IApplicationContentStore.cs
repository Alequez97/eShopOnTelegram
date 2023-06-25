namespace eShopOnTelegram.ApplicationContent.Interfaces;

public interface IApplicationContentStore
{
    Task<string> GetApplicationContentAsJsonStringAsync(CancellationToken cancellationToken);

    Task<string> GetValueAsync(string key, CancellationToken cancellationToken);

    Task<bool> UpdateContentAsync(Dictionary<string, string> keyValues, CancellationToken cancellationToken);
}
