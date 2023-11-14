using eShopOnTelegram.Admin.Constants;
using eShopOnTelegram.RuntimeConfiguration.Secrets.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.Secrets.Requests;

using Microsoft.AspNetCore.Authorization;

namespace eShopOnTelegram.Admin.Endpoints.Secrets;

public class CreateOrUpdateSecret : EndpointBaseAsync
    .WithRequest<CreateOrUpdateSecretRequest>
    .WithActionResult
{
    private readonly IKeyVaultClient _keyVaultClient;
    private readonly ILogger<CreateOrUpdateSecret> _logger;

    public CreateOrUpdateSecret(
        IKeyVaultClient keyVaultClient,
        ILogger<CreateOrUpdateSecret> logger)
    {
        _keyVaultClient = keyVaultClient;
        _logger = logger;
    }


    [Authorize(Policy = AuthPolicy.RequireSuperadminClaim)]
    [HttpPost("/api/secretsConfig")]
    [SwaggerOperation(Tags = new[] { SwaggerGroup.Secrets })]
    public override async Task<ActionResult> HandleAsync(CreateOrUpdateSecretRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await _keyVaultClient.CreateOrUpdateAsync(request);

            // TODO: Restart shop application

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest();
        }
    }
}
