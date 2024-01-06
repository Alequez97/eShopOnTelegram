namespace eShopOnTelegram.Utils.AzureServiceManager.Interfaces;

public interface IAzureAppServiceManager
{
	Task RestartAppServiceAsync(string resourceGroup, string appServiceName, CancellationToken cancellationToken);
}
