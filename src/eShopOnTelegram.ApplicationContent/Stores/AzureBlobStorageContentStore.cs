using System.Text;

using Azure.Storage.Blobs;

using eShopOnTelegram.ApplicationContent.Interfaces;
using eShopOnTelegram.ApplicationContent.Models;

using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace eShopOnTelegram.ApplicationContent.Stores;

public class AzureBlobStorageApplicationContentStore : IApplicationContentStore
{
    private readonly BlobContainerClient _blobContainerClient;
    private const string _applicationContentFileName = "application-content.json";
    private readonly IApplicationDefaultContentStore _applicationDefaultContentStore;

    public AzureBlobStorageApplicationContentStore(
        IConfiguration configuration,
        IApplicationDefaultContentStore applicationDefaultContentStore
        )
    {
        var connectionString = configuration["Azure:StorageAccountConnectionString"];
        var blobContainerName = configuration["Azure:ApplicationContentBlobContainerName"];

        var blobServiceClient = new BlobServiceClient(connectionString);
        _blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);
        _applicationDefaultContentStore = applicationDefaultContentStore;
    }

    public async Task<ApplicationContentModel> GetApplicationContentAsync(CancellationToken cancellationToken)
    {
        // TODO: Implement exception handling mechanism

        var applicationContentJsonAsString = await ReadApplicationContentFromBlobContainerAsync(cancellationToken);

        return JsonConvert.DeserializeObject<ApplicationContentModel>(applicationContentJsonAsString);
    }

    public async Task<string> GetSingleValueAsync(string key, CancellationToken cancellationToken)
    {
        // TODO: Implement exception handling mechanism

        var applicationContentJsonAsString = await ReadApplicationContentFromBlobContainerAsync(cancellationToken);

        var data = JObject.Parse(applicationContentJsonAsString);
        var value = data.SelectToken(key)?.ToString();

        return !string.IsNullOrWhiteSpace(value) ? value : await _applicationDefaultContentStore.GetApplicationDefaultValueAsync(key, cancellationToken);
    }

    public async Task UpdateContentAsync(Dictionary<string, string> keyValues, CancellationToken cancellationToken)
    {
        // TODO: Implement exception handling mechanism

        var applicationContentJson = await ReadApplicationContentFromBlobContainerAsync(cancellationToken);
        var data = JObject.Parse(applicationContentJson);

        foreach (var keyValue in keyValues)
        {
            var tokenProperty = data.SelectToken(keyValue.Key);

            if (tokenProperty != null && tokenProperty is JValue keyToUpdate)
            {
                keyToUpdate.Value = keyValue.Value;
            }
        }

        await UploadApplicationContentToBlobContainerAsync(data.ToString(), cancellationToken);

        await Task.CompletedTask;
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
