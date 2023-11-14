using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.AppService;
using Azure.ResourceManager.Resources;

using eShopOnTelegram.Utils.AzureServiceManager.Interfaces;
using eShopOnTelegram.Utils.Configuration;

namespace eShopOnTelegram.Utils.AzureServiceManager;
public class AzureAppServiceManager : IAzureAppServiceManager
{
    private readonly ArmClient _armClient;

    public AzureAppServiceManager(AppSettings appSettings)
    {
        var azureSettings = appSettings.AzureSettings;
        var credentials = new ClientSecretCredential(azureSettings.TenantId, azureSettings.ClientId, azureSettings.ClientSecret);

        _armClient = new ArmClient(credentials);
    }

    public async Task RestartAppServiceAsync(string resourceGroupName, string appServiceName, CancellationToken cancellationToken)
    {
        SubscriptionResource subscription = await _armClient.GetDefaultSubscriptionAsync(cancellationToken);
        ResourceGroupCollection resourceGroups = subscription.GetResourceGroups();
        ResourceGroupResource resourceGroup = await resourceGroups.GetAsync(resourceGroupName, cancellationToken);

        WebSiteResource appService = await resourceGroup.GetWebSiteAsync(appServiceName, cancellationToken);

        await appService.RestartAsync(cancellationToken: cancellationToken);
    }
}
