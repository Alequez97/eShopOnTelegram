using eShopOnTelegram.Admin.Constants;
using eShopOnTelegram.Admin.Extensions;
using eShopOnTelegram.Domain.Requests.Products;
using eShopOnTelegram.Domain.Responses;

namespace eShopOnTelegram.Admin.Endpoints.Products;

public class UpdateProduct : EndpointBaseAsync
    .WithRequest<UpdateProductRequest>
    .WithActionResult<ActionResponse>
{
    private readonly IProductService _productService;

    public UpdateProduct(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPut("/api/products/{id}")]
    [SwaggerOperation(Tags = new[] { SwaggerGroup.Products })]
    public override async Task<ActionResult<ActionResponse>> HandleAsync([FromBody] UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _productService.UpdateAsync(request, cancellationToken);

        return response.AsActionResult();
    }
}
