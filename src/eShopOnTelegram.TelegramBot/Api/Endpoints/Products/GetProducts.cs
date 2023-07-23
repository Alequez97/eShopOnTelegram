using eShopOnTelegram.Domain.Dto.Products;
using eShopOnTelegram.TelegramBot.Api.Constants;
using eShopOnTelegram.TelegramBot.Api.Extensions;

namespace eShopOnTelegram.TelegramBot.Api.Endpoints.Products;

public class GetProducts : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<IEnumerable<ProductDto>>
{
    private readonly IProductService _productService;

    public GetProducts(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("/api/products")]
    [SwaggerOperation(Tags = new[] { SwaggerGroup.Products })]
    public async override Task<ActionResult<IEnumerable<ProductDto>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var response = await _productService.GetAllAsync(cancellationToken);
        return response.AsActionResult();
    }
}