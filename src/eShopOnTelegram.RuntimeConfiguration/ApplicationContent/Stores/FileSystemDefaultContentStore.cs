using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Content;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Stores;

public class FileSystemDefaultContentStore : IApplicationDefaultContentStore
{
    public async Task<ApplicationContentModel> GetDefaultApplicationContentAsync(CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(new ApplicationContentModel());
    }

    public async Task<string> GetDefaultValueAsync(string key, CancellationToken cancellationToken)
    {
        var applicationContentModel = await GetDefaultApplicationContentAsync(cancellationToken);

        var data = JObject.Parse(JsonConvert.SerializeObject(applicationContentModel));
        var value = data[key]?.ToString();

        return value;
    }
}
