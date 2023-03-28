using eShopOnTelegram.Admin.Constants;
using eShopOnTelegram.Admin.Extensions;
using eShopOnTelegram.Domain.Requests;
using eShopOnTelegram.Domain.Responses.ProductCategories;
using eShopOnTelegram.Domain.Services.Interfaces;

namespace eShopOnTelegram.Admin.Endpoints.ProductCategories;

public class GetProductCategories : EndpointBaseAsync
    .WithRequest<GetRequest>
    .WithActionResult<IEnumerable<GetProductCategoryResponse>>
{
    private readonly IProductCategoryService _productCategoryService;

    public GetProductCategories(IProductCategoryService productCategoryService)
    {
        _productCategoryService = productCategoryService;
    }

    [HttpGet("/api/productCategories")]
    [SwaggerOperation(Tags = new[] { SwaggerGroup.ProductCategories })]
    public override async Task<ActionResult<IEnumerable<GetProductCategoryResponse>>> HandleAsync([FromQuery] GetRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _productCategoryService.GetMultipleAsync(request, cancellationToken);

        Response.AddPaginationHeaders(request.PaginationModel, response.TotalItemsInDatabase);

        return response.AsActionResult();
    }
}
