namespace eShopOnTelegram.TelegramBot.Appsettings;

public class AzureAppsettings
{
    public required string StorageAccountConnectionString { get; set; }

    public required string ApplicationContentBlobContainerName { get; set; }

    public required string ApplicationContentFileName { get; set; }
}
