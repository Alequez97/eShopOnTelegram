using System.Text;

using Azure.Storage.Blobs;

using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Content;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
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

    public async Task<ApplicationContentModel> GetApplicationContentAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await ReadApplicationContentFromBlobContainerAsync(cancellationToken);
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
            var applicationContentModel = await ReadApplicationContentFromBlobContainerAsync(cancellationToken);

            var data = JObject.Parse(JsonConvert.SerializeObject(applicationContentModel));
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
            var applicationContentModel = await ReadApplicationContentFromBlobContainerAsync(cancellationToken);
            var data = JObject.Parse(JsonConvert.SerializeObject(applicationContentModel));

            foreach (var keyValue in keyValues)
            {
                data[keyValue.Key] = keyValue.Value;
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

    private async Task<ApplicationContentModel> ReadApplicationContentFromBlobContainerAsync(CancellationToken cancellationToken)
    {
        var defaultApplicatonContent = await _applicationDefaultContentStore.GetDefaultApplicationContentAsync(cancellationToken);

        var blobClient = _blobContainerClient.GetBlobClient(_applicationContentFileName);
        var blobExists = await blobClient.ExistsAsync(cancellationToken);
        if (!blobExists)
        {

            await UploadApplicationContentToBlobContainerAsync(JsonConvert.SerializeObject(defaultApplicatonContent), cancellationToken);
            return defaultApplicatonContent;
        }

        using var applicationContentBlobStorageMemoryStream = new MemoryStream();
        await blobClient.DownloadToAsync(applicationContentBlobStorageMemoryStream, cancellationToken);

        applicationContentBlobStorageMemoryStream.Position = 0;

        using var applicationContentBlobStorageReader = new StreamReader(applicationContentBlobStorageMemoryStream);
        var applicationContentFromAzureBlobStorage = await applicationContentBlobStorageReader.ReadToEndAsync(cancellationToken);
        var applicationContentModel = JsonConvert.DeserializeObject<ApplicationContentModel>(applicationContentFromAzureBlobStorage);

        var defaultObject = JObject.Parse(JsonConvert.SerializeObject(defaultApplicatonContent));
        var azureBlobStorageObject = JObject.Parse(applicationContentFromAzureBlobStorage);

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

            applicationContentFromAzureBlobStorage = azureBlobStorageObject.ToString();

            // Upload updated JSON to Azure Blob Storage
            applicationContentBlobStorageMemoryStream.Position = 0;
            using var writer = new StreamWriter(applicationContentBlobStorageMemoryStream);
            await writer.WriteAsync(applicationContentFromAzureBlobStorage);
            await writer.FlushAsync();
            applicationContentBlobStorageMemoryStream.Position = 0;
            await blobClient.UploadAsync(applicationContentBlobStorageMemoryStream, true, cancellationToken);
        }

        return applicationContentModel;
    }

    private async Task UploadApplicationContentToBlobContainerAsync(string contentJson, CancellationToken cancellationToken)
    {
        var blobClient = _blobContainerClient.GetBlobClient(_applicationContentFileName);

        using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(contentJson));
        await blobClient.UploadAsync(memoryStream, overwrite: true, cancellationToken: cancellationToken);
    }
}
