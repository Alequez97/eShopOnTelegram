using System.Text;

using Azure.Storage.Blobs;

using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json.Linq;

namespace eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Stores;

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
        var blobContainerName = configuration["Azure:RuntimeConfigurationBlobContainerName"];

        var blobServiceClient = new BlobServiceClient(connectionString);
        _blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);
        _applicationDefaultContentStore = applicationDefaultContentStore;
        _logger = logger;
    }

    public async Task<string> GetApplicationContentAsJsonStringAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await ReadApplicationContentFromBlobContainerAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            return await _applicationDefaultContentStore.GetDefaultApplicationContentAsJsonStringAsync(cancellationToken);
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
            throw new Exception("Test application insights");

            //var applicationContentJson = await ReadApplicationContentFromBlobContainerAsync(cancellationToken);
            //var data = JObject.Parse(applicationContentJson);

            //foreach (var keyValue in keyValues)
            //{
            //    data[keyValue.Key] = keyValue.Value;
            //}

            //await UploadApplicationContentToBlobContainerAsync(data.ToString(), cancellationToken);

            //return true;
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
        var defaultApplicatonContent = await _applicationDefaultContentStore.GetDefaultApplicationContentAsJsonStringAsync(cancellationToken);

        using var memoryStream = new MemoryStream();

        var blobExists = await blobClient.ExistsAsync(cancellationToken);
        if (!blobExists)
        {
            await UploadApplicationContentToBlobContainerAsync(defaultApplicatonContent, cancellationToken);
        }

        await blobClient.DownloadToAsync(memoryStream, cancellationToken);

        memoryStream.Position = 0;

        using var reader = new StreamReader(memoryStream);
        var downloadedJsonContent = await reader.ReadToEndAsync(cancellationToken);

        var defaultObject = JObject.Parse(defaultApplicatonContent);
        var azureBlobStorageObject = JObject.Parse(downloadedJsonContent);

        // Check if any new keys are missing in the azure blob storage JSON
        var missingKeys = defaultObject.Properties()
            .Where(p => !azureBlobStorageObject.ContainsKey(p.Name))
            .ToList();

        if (missingKeys.Any())
        {
            var propertiesToInsert = missingKeys.Select(jsonProperty =>
            {
                var defaultValue = jsonProperty.Value;
                return new JProperty(jsonProperty.Name, defaultValue);
            });

            // Find the appropriate position to insert the missing keys
            var defaultObjectPropertiesList = defaultObject.Properties().ToList();
            var azureBlobStorageObjectPropertiesList = azureBlobStorageObject.Properties().ToList();

            foreach (var missingKey in missingKeys)
            {
                var keyIndex = defaultObjectPropertiesList.IndexOf(missingKey);
                if (keyIndex >= 0)
                {
                    azureBlobStorageObjectPropertiesList.Insert(keyIndex, missingKey);
                }
            }

            // Create a new JObject with the updated properties
            azureBlobStorageObject = new JObject(azureBlobStorageObjectPropertiesList);

            downloadedJsonContent = azureBlobStorageObject.ToString();

            // Upload updated JSON to Azure Blob Storage
            memoryStream.Position = 0;
            using var writer = new StreamWriter(memoryStream);
            await writer.WriteAsync(downloadedJsonContent);
            await writer.FlushAsync();
            memoryStream.Position = 0;
            await blobClient.UploadAsync(memoryStream, true, cancellationToken);
        }

        return downloadedJsonContent;
    }

    private async Task UploadApplicationContentToBlobContainerAsync(string contentJson, CancellationToken cancellationToken)
    {
        var blobClient = _blobContainerClient.GetBlobClient(_applicationContentFileName);

        using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(contentJson));
        await blobClient.UploadAsync(memoryStream, overwrite: true, cancellationToken: cancellationToken);
    }
}
