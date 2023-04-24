using eShopOnTelegram.Admin.Constants;
using eShopOnTelegram.Admin.Extensions;
using eShopOnTelegram.Domain.Dto.Products;

namespace eShopOnTelegram.Admin.Endpoints.Products;

public class GetProductById : EndpointBaseAsync
    .WithRequest<long>
    .WithActionResult<ProductDto>
{
    private readonly IProductService _productService;

    public GetProductById(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("/api/products/{id}")]
    [SwaggerOperation(Tags = new[] { SwaggerGroup.Products })]
    public override async Task<ActionResult<ProductDto>> HandleAsync([FromRoute] long id, CancellationToken cancellationToken = default)
    {
        var response = await _productService.GetAsync(id, cancellationToken);

        return response.AsActionResult();
    }
}
