﻿using eShopOnTelegram.Admin.Constants;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;

namespace eShopOnTelegram.Admin.Endpoints.ApplicationContent;

public class UpdateApplicationContent : EndpointBaseAsync
    .WithRequest<Dictionary<string, string>>
    .WithActionResult
{
    private readonly IApplicationContentStore _applicationContentStore;

    public UpdateApplicationContent(IApplicationContentStore applicationContentStore)
    {
        _applicationContentStore = applicationContentStore;
    }

    [HttpPatch("/api/applicationContent")]
    [SwaggerOperation(Tags = new[] { SwaggerGroup.ApplicationContent })]
    public override async Task<ActionResult> HandleAsync(Dictionary<string, string> request, CancellationToken cancellationToken)
    {
        var uploadSuccessfull = await _applicationContentStore.UpdateContentAsync(request, cancellationToken);

        return uploadSuccessfull ? Ok() : StatusCode(503);
    }
}
