using System.Text;

using Azure.Storage.Blobs;

using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Stores;
using eShopOnTelegram.RuntimeConfiguration.BotOwnerData.Interfaces;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace eShopOnTelegram.RuntimeConfiguration.BotOwnerData.Stores;

public class AzureBlobStorageBotOwnerDataStore : IBotOwnerDataStore
{
    private const string _botOwnerGroupIdFileName = "bot-owner-group-id.txt";
    private readonly BlobContainerClient _blobContainerClient;
    private readonly ILogger<AzureBlobStorageApplicationContentStore> _logger;

    public AzureBlobStorageBotOwnerDataStore(
        IConfiguration configuration,
        ILogger<AzureBlobStorageApplicationContentStore> logger)
    {
        var connectionString = configuration["Azure:StorageAccountConnectionString"];
        var blobContainerName = configuration["Azure:RuntimeConfigurationBlobContainerName"];

        var blobServiceClient = new BlobServiceClient(connectionString);
        _blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);

        _logger = logger;
    }

    public async Task<string> GetBotOwnerTelegramGroupIdAsync(CancellationToken cancellationToken)
    {
        var blobClient = _blobContainerClient.GetBlobClient(_botOwnerGroupIdFileName);

        using var memoryStream = new MemoryStream();

        var blobExists = await blobClient.ExistsAsync(cancellationToken);
        if (!blobExists)
        {
            return string.Empty;
        }

        await blobClient.DownloadToAsync(memoryStream, cancellationToken);
        memoryStream.Position = 0;

        using var reader = new StreamReader(memoryStream);
        return await reader.ReadToEndAsync(cancellationToken);
    }

    public async Task<bool> SaveBotOwnerTelegramGroupIdAsync(string telegramGroupId, CancellationToken cancellationToken)
    {
        try
        {
            var blobClient = _blobContainerClient.GetBlobClient(_botOwnerGroupIdFileName);

            using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(telegramGroupId));
            await blobClient.UploadAsync(memoryStream, overwrite: false, cancellationToken: cancellationToken);

            return true;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            return false;
        }
    }
}