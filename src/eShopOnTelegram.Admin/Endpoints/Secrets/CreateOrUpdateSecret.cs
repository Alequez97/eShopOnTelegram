using eShopOnTelegram.Admin.Constants;
using eShopOnTelegram.RuntimeConfiguration.Secrets.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.Secrets.Requests;
using eShopOnTelegram.Utils.AzureServiceManager.Interfaces;
using eShopOnTelegram.Utils.Configuration;

using Microsoft.AspNetCore.Authorization;

namespace eShopOnTelegram.Admin.Endpoints.Secrets;

public class CreateOrUpdateSecret : EndpointBaseAsync
	.WithRequest<CreateOrUpdateSecretRequest>
	.WithActionResult
{
	private readonly IKeyVaultClient _keyVaultClient;
	private readonly IAzureAppServiceManager _azureAppServiceManager;
	private readonly AppSettings _appSettings;
	private readonly ILogger<CreateOrUpdateSecret> _logger;

	public CreateOrUpdateSecret(
		IKeyVaultClient keyVaultClient,
		IAzureAppServiceManager azureAppServiceManager,
		AppSettings appSettings,
		ILogger<CreateOrUpdateSecret> logger)
	{
		_keyVaultClient = keyVaultClient;
		_azureAppServiceManager = azureAppServiceManager;
		_appSettings = appSettings;
		_logger = logger;
	}


	[Authorize(Policy = AuthPolicy.RequireSuperadminClaim)]
	[HttpPost("/api/secretsConfig")]
	[SwaggerOperation(Tags = new[] { SwaggerGroup.Secrets })]
	public override async Task<ActionResult> HandleAsync(CreateOrUpdateSecretRequest request, CancellationToken cancellationToken)
	{
		try
		{
			await _keyVaultClient.CreateOrUpdateAsync(request);

			await _azureAppServiceManager.RestartAppServiceAsync(_appSettings.AzureSettings.ResourceGroupName, _appSettings.AzureSettings.ShopAppServiceName, cancellationToken);

			return Ok();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, ex.Message);
			return BadRequest();
		}
	}
}
