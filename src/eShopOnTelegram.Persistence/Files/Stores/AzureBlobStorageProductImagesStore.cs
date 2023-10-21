using Azure.Storage.Blobs;

using eShopOnTelegram.Persistence.Files.Interfaces;

namespace eShopOnTelegram.Persistence.Files.Stores;

public class AzureBlobStorageProductImagesStore : IProductImagesStore
{
    private readonly BlobContainerClient _blobContainer;

    public AzureBlobStorageProductImagesStore(string connectionString, string blobContainerName)
    {
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

    public async Task DeleteAsync(string id, CancellationToken cancellationToken)
    {
        await _blobContainer.DeleteBlobIfExistsAsync(id, cancellationToken: cancellationToken);
    }
}
