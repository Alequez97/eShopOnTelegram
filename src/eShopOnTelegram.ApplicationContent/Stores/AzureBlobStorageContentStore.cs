using Azure.Storage.Blobs;

using eShopOnTelegram.ApplicationContent.Interfaces;
using eShopOnTelegram.ApplicationContent.Models;

using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;

namespace eShopOnTelegram.ApplicationContent.Providers;

public class AzureBlobStorageApplicationContentStore : IApplicationContentStore
{
    private readonly BlobContainerClient _blobContainerClient;
    private const string _applicationContentFileName = "application-content.jsonAsString";

    public AzureBlobStorageApplicationContentStore(IConfiguration configuration)
    {
        var connectionString = configuration["Azure:StorageAccountConnectionString"];
        var blobContainerName = configuration["Azure:ApplicationContentBlobContainerName"];

        var blobServiceClient = new BlobServiceClient(connectionString);
        _blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);
    }

    public async Task<ApplicationContentModel> GetApplicationContentAsync(CancellationToken cancellationToken)
    {
        var blobClient = _blobContainerClient.GetBlobClient(_applicationContentFileName);

        using var memoryStream = new MemoryStream();
        await blobClient.DownloadToAsync(memoryStream, cancellationToken);

        memoryStream.Position = 0;

        using var reader = new StreamReader(memoryStream);
        var jsonAsString = await reader.ReadToEndAsync(cancellationToken);
        var applicationContent = JsonConvert.DeserializeObject<ApplicationContentModel>(jsonAsString);

        return applicationContent;
    }

    public Task<string> GetSingleValueAsync(string key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateContentAsync(List<KeyValuePair<string, string>> keyValues, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
