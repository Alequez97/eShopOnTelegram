using Azure.Storage.Blobs;

using eShopOnTelegram.Persistence.Files.Interfaces;

using Microsoft.AspNetCore.Http;
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

    public async Task<string> SaveAsync(IFormFile image, CancellationToken cancellationToken)
    {
        var imageName = $"{Guid.NewGuid()}.png";
        var blobClient = _blobContainer.GetBlobClient(imageName);

        await using var imageStream = image.OpenReadStream();
        await blobClient.UploadAsync(imageStream, cancellationToken);

        return imageName;
    }

    public async Task DeleteAsync(string id)
    {
        await _blobContainer.DeleteBlobIfExistsAsync(id);
    }
}
