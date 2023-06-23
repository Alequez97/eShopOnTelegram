using System.Reflection;

using eShopOnTelegram.ApplicationContent.Interfaces;
using eShopOnTelegram.ApplicationContent.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace eShopOnTelegram.ApplicationContent.Stores;

public class FileSystemApplicationDefaultValueContentStore : IApplicationDefaultContentStore
{
    public async Task<ApplicationContentModel> GetDefaultApplicationContentAsync(CancellationToken cancellationToken)
    {
        var jsonAsString = await GetDefaultContentJsonStringAsync(cancellationToken);

        return JsonConvert.DeserializeObject<ApplicationContentModel>(jsonAsString);
    }

    public async Task<string> GetDefaultValueAsync(string key, CancellationToken cancellationToken)
    {
        var jsonAsString = await GetDefaultContentJsonStringAsync(cancellationToken);

        var data = JObject.Parse(jsonAsString);
        var value = data.SelectToken(key)?.ToString();

        return value;
    }

    private async Task<string> GetDefaultContentJsonStringAsync(CancellationToken cancellationToken)
    {
        var applicationContentDefaultFileName = "application-content-defaults.json";
        var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        var fileLocation = $"{appLocation}/Content/{applicationContentDefaultFileName}";

        return await File.ReadAllTextAsync(fileLocation, cancellationToken);
    }
}
