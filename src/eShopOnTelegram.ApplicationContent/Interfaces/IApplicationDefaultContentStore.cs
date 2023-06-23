namespace eShopOnTelegram.ApplicationContent.Interfaces;

public interface IApplicationDefaultContentStore
{
    Task<string> GetApplicationDefaultValueAsync(string key, CancellationToken cancellationToken);
}
