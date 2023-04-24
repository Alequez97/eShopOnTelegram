using eShopOnTelegram.Admin.Constants;
using eShopOnTelegram.Admin.Extensions;
using eShopOnTelegram.Domain.Requests.ProductCategories;

namespace eShopOnTelegram.Admin.Endpoints.ProductCategories;

public class UpdateProductCategory : EndpointBaseAsync
    .WithRequest<UpdateProductCategoryRequest>
    .WithActionResult
{
    private readonly IProductCategoryService _productCategoryService;

    public UpdateProductCategory(IProductCategoryService productCategoryService)
    {
        _productCategoryService = productCategoryService;
    }

    [HttpPut("/api/productCategories/{id}")]
    [SwaggerOperation(Tags = new[] { SwaggerGroup.ProductCategories })]
    public override async Task<ActionResult> HandleAsync([FromBody] UpdateProductCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _productCategoryService.UpdateAsync(request, cancellationToken);

        return response.AsActionResult();
    }
}
