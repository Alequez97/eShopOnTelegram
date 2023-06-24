using System.Text;

using Azure.Storage.Blobs;

using eShopOnTelegram.ApplicationContent.Interfaces;
using eShopOnTelegram.ApplicationContent.Models;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace eShopOnTelegram.ApplicationContent.Stores;

public class AzureBlobStorageApplicationContentStore : IApplicationContentStore
{
    private const string _applicationContentFileName = "application-content.json";

    private readonly BlobContainerClient _blobContainerClient;
    private readonly IApplicationDefaultContentStore _applicationDefaultContentStore;
    private readonly ILogger<AzureBlobStorageApplicationContentStore> _logger;

    public AzureBlobStorageApplicationContentStore(
        IConfiguration configuration,
        IApplicationDefaultContentStore applicationDefaultContentStore,
        ILogger<AzureBlobStorageApplicationContentStore> logger
        )
    {
        var connectionString = configuration["Azure:StorageAccountConnectionString"];
        var blobContainerName = configuration["Azure:ApplicationContentBlobContainerName"];

        var blobServiceClient = new BlobServiceClient(connectionString);
        _blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);
        _applicationDefaultContentStore = applicationDefaultContentStore;
        _logger = logger;
    }

    public async Task<ApplicationContentModel> GetApplicationContentAsync(CancellationToken cancellationToken)
    {
        try
        {
            var applicationContentJsonAsString = await ReadApplicationContentFromBlobContainerAsync(cancellationToken);

            return JsonConvert.DeserializeObject<ApplicationContentModel>(applicationContentJsonAsString);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            return await _applicationDefaultContentStore.GetDefaultApplicationContentAsync(cancellationToken);
        }
    }

    public async Task<string> GetValueAsync(string key, CancellationToken cancellationToken)
    {
        try
        {
            var applicationContentJsonAsString = await ReadApplicationContentFromBlobContainerAsync(cancellationToken);

            var data = JObject.Parse(applicationContentJsonAsString);
            var value = data[key]?.ToString();

            return !string.IsNullOrWhiteSpace(value) ? value : await _applicationDefaultContentStore.GetDefaultValueAsync(key, cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            return await _applicationDefaultContentStore.GetDefaultValueAsync(key, cancellationToken);
        }
    }

    public async Task<bool> UpdateContentAsync(Dictionary<string, string> keyValues, CancellationToken cancellationToken)
    {
        try
        {
            var applicationContentJson = await ReadApplicationContentFromBlobContainerAsync(cancellationToken);
            var data = JObject.Parse(applicationContentJson);

            foreach (var keyValue in keyValues)
            {
                var value = data[keyValue.Key];
                if (value != null)
                {
                    data[keyValue.Key] = keyValue.Value;
                }
            }

            await UploadApplicationContentToBlobContainerAsync(data.ToString(), cancellationToken);

            return true;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            return false;
        }
    }

    private async Task<string> ReadApplicationContentFromBlobContainerAsync(CancellationToken cancellationToken)
    {
        var blobClient = _blobContainerClient.GetBlobClient(_applicationContentFileName);

        using var memoryStream = new MemoryStream();
        await blobClient.DownloadToAsync(memoryStream, cancellationToken);

        memoryStream.Position = 0;

        using var reader = new StreamReader(memoryStream);
        var jsonAsString = await reader.ReadToEndAsync(cancellationToken);

        return jsonAsString;
    }

    private async Task UploadApplicationContentToBlobContainerAsync(string contentJson, CancellationToken cancellationToken)
    {
        var blobClient = _blobContainerClient.GetBlobClient(_applicationContentFileName);

        using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(contentJson));
        await blobClient.UploadAsync(memoryStream, overwrite: true, cancellationToken: cancellationToken);
    }
}
