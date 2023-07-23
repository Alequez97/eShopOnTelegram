namespace eShopOnTelegram.TelegramBot.Worker.Appsettings;

public class AzureAppsettings
{
    public required string StorageAccountConnectionString { get; set; }

    public required string RuntimeConfigurationBlobContainerName { get; set; }

    public required string ApplicationContentFileName { get; set; }
}
