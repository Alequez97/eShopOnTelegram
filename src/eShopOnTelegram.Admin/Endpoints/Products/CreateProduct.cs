using eShopOnTelegram.Admin.Constants;
using eShopOnTelegram.Admin.Extensions;
using eShopOnTelegram.Domain.Requests.Products;
using eShopOnTelegram.Domain.Responses;

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
    public override async Task<ActionResult<Response>> HandleAsync([FromForm] CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _productService.CreateAsync(request, cancellationToken);
        return response.AsActionResult();
    }
}
