using System.Reflection;

using eShopOnTelegram.ApplicationContent.Interfaces;

using Newtonsoft.Json.Linq;

namespace eShopOnTelegram.ApplicationContent.Stores;

public class FileSystemDefaultContentStore : IApplicationDefaultContentStore
{
    public async Task<string> GetDefaultApplicationContentAsJsonStringAsync(CancellationToken cancellationToken)
    {
        var applicationContentDefaultFileName = "application-content-defaults.json";
        var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        var fileLocation = $"{appLocation}/Content/{applicationContentDefaultFileName}";

        return await File.ReadAllTextAsync(fileLocation, cancellationToken);
    }

    public async Task<string> GetDefaultValueAsync(string key, CancellationToken cancellationToken)
    {
        var jsonAsString = await GetDefaultApplicationContentAsJsonStringAsync(cancellationToken);

        var data = JObject.Parse(jsonAsString);
        var value = data[key]?.ToString();

        return value;
    }
}
