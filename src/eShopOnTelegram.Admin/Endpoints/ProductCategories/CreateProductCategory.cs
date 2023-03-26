using eShopOnTelegram.Admin.Constants;
using eShopOnTelegram.Admin.Extensions;
using eShopOnTelegram.Domain.Requests.ProductCategories;
using eShopOnTelegram.Domain.Responses;

namespace eShopOnTelegram.Admin.Endpoints.ProductCategories;

public class CreateProductCategory : EndpointBaseAsync
    .WithRequest<CreateProductCategoryRequest>
    .WithActionResult<Response>
{
    private readonly IProductCategoryService _productCategoryService;

    public CreateProductCategory(IProductCategoryService productCategoryService)
    {
        _productCategoryService = productCategoryService;
    }

    [HttpPost("/api/productCategories")]
    [SwaggerOperation(Tags = new[] { SwaggerGroup.ProductCategories })]
    public override async Task<ActionResult<Response>> HandleAsync([FromBody] CreateProductCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _productCategoryService.CreateAsync(request, cancellationToken);
        return response.AsActionResult();
    }
}
