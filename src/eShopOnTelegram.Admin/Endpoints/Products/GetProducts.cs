using eShopOnTelegram.Admin.Constants;
using eShopOnTelegram.Admin.Extensions;
using eShopOnTelegram.Domain.Requests;

namespace VirtualCards.Admin.Endpoints.Customers;

public class GetProducts : EndpointBaseAsync
    .WithRequest<GetRequest>
    .WithActionResult<IEnumerable<GetProductResponse>>
{
    private readonly IProductService _productService;

    public GetProducts(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("/api/products")]
    [SwaggerOperation(Tags = new[] { SwaggerGroup.Products })]
    public async override Task<ActionResult<IEnumerable<GetProductResponse>>> HandleAsync([FromQuery] GetRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _productService.GetMultipleAsync(request, cancellationToken);

        Response.AddPaginationHeaders(request.PaginationModel, response.TotalItemsInDatabase);

        return response.AsActionResult();
    }
}