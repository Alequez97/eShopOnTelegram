using System.Text;

using Azure.Storage.Blobs;

using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Stores;
using eShopOnTelegram.RuntimeConfiguration.BotOwnerData.Interfaces;
using eShopOnTelegram.Utils.Configuration;

using Microsoft.Extensions.Logging;

namespace eShopOnTelegram.RuntimeConfiguration.BotOwnerData.Stores;

public class AzureBlobStorageBotOwnerDataStore : IBotOwnerDataStore
{
    private const string BOT_OWNER_GROUP_ID_FILE_NAME = "bot-owner-group-id.txt";
    private readonly BlobContainerClient _blobContainerClient;
    private readonly string _botOwnerTelegramId;
    private readonly ILogger<AzureBlobStorageApplicationContentStore> _logger;

    public AzureBlobStorageBotOwnerDataStore(
        AppSettings appSettings,
        ILogger<AzureBlobStorageApplicationContentStore> logger)
    {
        var blobServiceClient = new BlobServiceClient(appSettings.AzureSettings.StorageAccountConnectionString);
        _blobContainerClient = blobServiceClient.GetBlobContainerClient(appSettings.AzureSettings.RuntimeConfigurationBlobContainerName);

        _botOwnerTelegramId = appSettings.TelegramBotSettings.BotOwnerTelegramId;

        _logger = logger;
    }

    public async Task<string?> GetBotOwnerTelegramGroupIdAsync(CancellationToken cancellationToken)
    {
        var blobClient = _blobContainerClient.GetBlobClient(BOT_OWNER_GROUP_ID_FILE_NAME);

        using var memoryStream = new MemoryStream();

        var blobExists = await blobClient.ExistsAsync(cancellationToken);
        if (!blobExists)
        {
            return null;
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
            var blobClient = _blobContainerClient.GetBlobClient(BOT_OWNER_GROUP_ID_FILE_NAME);

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