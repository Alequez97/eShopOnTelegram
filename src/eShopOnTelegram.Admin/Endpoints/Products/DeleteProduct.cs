using eShopOnTelegram.Admin.Constants;
using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Admin.Extensions;

namespace eShopOnTelegram.Admin.Endpoints.Products;

public class DeleteProduct : EndpointBaseAsync
    .WithRequest<long>
    .WithActionResult<ActionResponse>
{
    private readonly IProductService _productService;

    public DeleteProduct(IProductService productService)
    {
        _productService = productService;
    }

    [HttpDelete("/api/products/{id}")]
    [SwaggerOperation(Tags = new[] { SwaggerGroup.ProductCategories })]
    public override async Task<ActionResult<ActionResponse>> HandleAsync([FromRoute] long id, CancellationToken cancellationToken)
    {
        var response = await _productService.DeleteAsync(id, cancellationToken);
        return response.AsActionResult();
    }
}
