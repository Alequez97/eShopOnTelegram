using eShopOnTelegram.Admin.Constants;
using eShopOnTelegram.Admin.Extensions;
using eShopOnTelegram.Domain.Requests;
using eShopOnTelegram.Domain.Responses.Orders;

namespace eShopOnTelegram.Admin.Endpoints.Customers;

public class GetOrders : EndpointBaseAsync
    .WithRequest<GetRequest>
    .WithActionResult<IEnumerable<GetOrdersResponse>>
{
    private readonly IOrderService _orderService;

    public GetOrders(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("/api/orders")]
    [SwaggerOperation(Tags = new[] { SwaggerGroup.Orders })]
    public override async Task<ActionResult<IEnumerable<GetOrdersResponse>>> HandleAsync([FromQuery] GetRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _orderService.GetMultipleAsync(request, cancellationToken);

        Response.AddPaginationHeaders(request.PaginationModel, response.TotalItemsInDatabase);

        return response.AsActionResult();
    }
}
