using eShopOnTelegram.Admin.Constants;
using eShopOnTelegram.Admin.Extensions;
using eShopOnTelegram.Domain.Dto.ProductsCategories;
using eShopOnTelegram.Domain.Requests;

namespace eShopOnTelegram.Admin.Endpoints.ProductCategories;

public class GetProductCategories : EndpointBaseAsync
    .WithRequest<GetRequest>
    .WithActionResult<IEnumerable<ProductCategoryDto>>
{
    private readonly IProductCategoryService _productCategoryService;

    public GetProductCategories(IProductCategoryService productCategoryService)
    {
        _productCategoryService = productCategoryService;
    }

    [HttpGet("/api/productCategories")]
    [SwaggerOperation(Tags = new[] { SwaggerGroup.ProductCategories })]
    public override async Task<ActionResult<IEnumerable<ProductCategoryDto>>> HandleAsync([FromQuery] GetRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _productCategoryService.GetMultipleAsync(request, cancellationToken);

        Response.AddPaginationHeaders(request.PaginationModel, response.TotalItemsInDatabase);

        return response.AsActionResult();
    }
}
