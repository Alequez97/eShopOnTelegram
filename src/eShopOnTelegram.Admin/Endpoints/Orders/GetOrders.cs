using eShopOnTelegram.Admin.Constants;
using eShopOnTelegram.Admin.Extensions;
using eShopOnTelegram.Domain.Dto.Orders;
using eShopOnTelegram.Domain.Requests;

namespace eShopOnTelegram.Admin.Endpoints.Customers;

public class GetOrders : EndpointBaseAsync
    .WithRequest<GetRequest>
    .WithActionResult<IEnumerable<OrderDto>>
{
    private readonly IOrderService _orderService;

    public GetOrders(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("/api/orders")]
    [SwaggerOperation(Tags = new[] { SwaggerGroup.Orders })]
    public override async Task<ActionResult<IEnumerable<OrderDto>>> HandleAsync([FromQuery] GetRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _orderService.GetMultipleAsync(request, cancellationToken);

        Response.AddPaginationHeaders(request.PaginationModel, response.TotalItemsInDatabase);

        return response.AsActionResult();
    }
}
