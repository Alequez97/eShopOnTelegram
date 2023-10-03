using Azure.Storage.Blobs;

using eShopOnTelegram.Persistence.Files.Interfaces;

using Microsoft.Extensions.Configuration;

namespace eShopOnTelegram.Persistence.Files.Stores;

public class AzureBlobStorageProductImagesStore : IProductImagesStore
{
    private readonly BlobContainerClient _blobContainer;

    public AzureBlobStorageProductImagesStore(IConfiguration configuration)
    {
        var connectionString = configuration["Azure:StorageAccountConnectionString"];
        var blobContainerName = configuration["Azure:ProductImagesBlobContainerName"];
        
        var blobServiceClient = new BlobServiceClient(connectionString);
        _blobContainer = blobServiceClient.GetBlobContainerClient(blobContainerName);
    }

    public async Task<string> SaveAsync(byte[] file, string fileName, CancellationToken cancellationToken)
    {
        var generatedFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
        var blobClient = _blobContainer.GetBlobClient(generatedFileName);

        using var memoryStream = new MemoryStream(file);
        await blobClient.UploadAsync(memoryStream, cancellationToken);

        return generatedFileName;
    }

    public async Task DeleteAsync(string id)
    {
        await _blobContainer.DeleteBlobIfExistsAsync(id);
    }
}
