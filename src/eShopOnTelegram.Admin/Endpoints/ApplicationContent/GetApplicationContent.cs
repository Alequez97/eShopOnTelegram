using eShopOnTelegram.Admin.Constants;
using eShopOnTelegram.ApplicationContent.Interfaces;

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
        return Ok(await _applicationContentStore.GetApplicationContentAsJsonStringAsync(cancellationToken));
    }
}
