using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Content;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.Translations.Interfaces;
using eShopOnTelegram.Utils.Configuration;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Stores;

public class FileSystemDefaultContentStore : IApplicationDefaultContentStore
{
	private readonly ITranslationsService _translationsService;
	private readonly AppSettings _appSettings;

	public FileSystemDefaultContentStore(
		ITranslationsService translationsService,
		AppSettings appSettings)
	{
		_translationsService = translationsService;
		_appSettings = appSettings;
	}

	public async Task<string> GetDefaultApplicationContentAsJsonStringAsync(CancellationToken cancellationToken)
	{
		return await Task.FromResult(JsonConvert.SerializeObject(new DefaultApplicationContentModel(_translationsService, _appSettings)));
	}

	public async Task<string> GetDefaultValueAsync(string key, CancellationToken cancellationToken)
	{
		var jsonAsString = await GetDefaultApplicationContentAsJsonStringAsync(cancellationToken);

		var data = JObject.Parse(jsonAsString);
		var value = data[key]?.ToString();

		return value;
	}
}
