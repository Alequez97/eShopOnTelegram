using eShopOnTelegram.Admin.Constants;
using eShopOnTelegram.RuntimeConfiguration.Secrets.Constants;

using Microsoft.AspNetCore.Authorization;

namespace eShopOnTelegram.Admin.Endpoints.Secrets;

public class GetSecretsConfig : EndpointBaseAsync
	.WithoutRequest
	.WithActionResult
{
	private readonly SecretsMappingConfig _secretsMappingConfig;

	public GetSecretsConfig(SecretsMappingConfig secretsMappingConfig)
	{
		_secretsMappingConfig = secretsMappingConfig;
	}

	[Authorize(Policy = AuthPolicy.RequireSuperadminClaim)]
	[HttpGet("/api/secretsConfig")]
	[SwaggerOperation(Tags = new[] { SwaggerGroup.Secrets })]
	public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
	{
		return Ok(_secretsMappingConfig.PublicConfig);
	}
}
