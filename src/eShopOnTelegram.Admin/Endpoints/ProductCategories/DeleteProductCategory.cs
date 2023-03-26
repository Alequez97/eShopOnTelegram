using eShopOnTelegram.Admin.Constants;
using eShopOnTelegram.Admin.Extensions;

namespace eShopOnTelegram.Admin.Endpoints.ProductCategories;

public class DeleteProductCategory : EndpointBaseAsync
    .WithRequest<long>
    .WithActionResult
{
    private readonly IProductCategoryService _productCategoryService;

    public DeleteProductCategory(IProductCategoryService productCategoryService)
    {
        _productCategoryService = productCategoryService;
    }

    [HttpDelete("/api/productCategories/{id}")]
    [SwaggerOperation(Tags = new[] { SwaggerGroup.ProductCategories })]
    public override async Task<ActionResult> HandleAsync([FromRoute] long id, CancellationToken cancellationToken = default)
    {
        var response = await _productCategoryService.DeleteAsync(id, cancellationToken);

        return response.AsActionResult();
    }
}
