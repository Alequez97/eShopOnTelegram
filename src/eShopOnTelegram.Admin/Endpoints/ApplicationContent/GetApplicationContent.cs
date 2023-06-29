using eShopOnTelegram.Admin.Constants;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;

using Newtonsoft.Json;

namespace eShopOnTelegram.Admin.Endpoints.ApplicationContent;

public class GetApplicationContent : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult
{
    private readonly IApplicationContentStore _applicationContentStore;

    public GetApplicationContent(IApplicationContentStore applicationContentStore)
    {
        _applicationContentStore = applicationContentStore;
    }

    [HttpGet("/api/applicationContent")]
    [SwaggerOperation(Tags = new[] { SwaggerGroup.ApplicationContent })]
    public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken)
    {
        return Ok(JsonConvert.SerializeObject(await _applicationContentStore.GetApplicationContentAsync(cancellationToken)));
    }
}
