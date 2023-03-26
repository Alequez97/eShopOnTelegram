using eShopOnTelegram.Domain.Requests.Products;
using eShopOnTelegram.Admin.Extensions;
using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.Admin.Constants;

namespace eShopOnTelegram.Admin.Endpoints.Products;

public class CreateProduct : EndpointBaseAsync
    .WithRequest<CreateProductRequest>
    .WithActionResult<Response>
{
    private readonly IProductService _productService;

    public CreateProduct(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost("/api/products")]
    [SwaggerOperation(Tags = new[] { SwaggerGroup.Products })]
    public override async Task<ActionResult<Response>> HandleAsync([FromBody] CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _productService.CreateAsync(request, cancellationToken);
        return response.AsActionResult();
    }
}
