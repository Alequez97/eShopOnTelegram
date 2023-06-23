using System.Reflection;

using eShopOnTelegram.ApplicationContent.Interfaces;

using Newtonsoft.Json.Linq;

namespace eShopOnTelegram.ApplicationContent.Stores;

public class FileSystemApplicationDefaultValueContentStore : IApplicationDefaultContentStore
{
    private const string _applicationContentDefaultFileName = "application-content-defaults.json";

    public async Task<string> GetApplicationDefaultValueAsync(string key, CancellationToken cancellationToken)
    {
        var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        var fileLocation = $"{appLocation}/Content/{_applicationContentDefaultFileName}";
        var jsonAsString = await File.ReadAllTextAsync(fileLocation, cancellationToken);

        var data = JObject.Parse(jsonAsString);
        var value = data.SelectToken(key)?.ToString();

        return value;
    }
}
