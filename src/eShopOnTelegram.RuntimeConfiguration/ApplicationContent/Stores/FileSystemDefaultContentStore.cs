using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Content;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Stores;

public class FileSystemDefaultContentStore : IApplicationDefaultContentStore
{
    public async Task<string> GetDefaultApplicationContentAsJsonStringAsync(CancellationToken cancellationToken)
    {
        return await Task.FromResult(JsonConvert.SerializeObject(new DefaultApplicationContentModel()));
    }

    public async Task<string> GetDefaultValueAsync(string key, CancellationToken cancellationToken)
    {
        var jsonAsString = await GetDefaultApplicationContentAsJsonStringAsync(cancellationToken);

        var data = JObject.Parse(jsonAsString);
        var value = data[key]?.ToString();

        return value;
    }
}
