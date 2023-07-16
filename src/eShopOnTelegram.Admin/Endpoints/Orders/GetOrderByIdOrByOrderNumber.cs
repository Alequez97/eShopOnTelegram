using eShopOnTelegram.Admin.Constants;
using eShopOnTelegram.Admin.Extensions;
using eShopOnTelegram.Domain.Dto.Orders;
using eShopOnTelegram.Domain.Responses;

namespace eShopOnTelegram.Admin.Endpoints.Orders;

public class GetOrderByIdOrByOrderNumber : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<OrderDto>
{
    private readonly IOrderService _orderService;

    public GetOrderByIdOrByOrderNumber(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("/api/orders/{idOrOrderNumber}")]
    [SwaggerOperation(Tags = new[] { SwaggerGroup.Orders })]
    public override async Task<ActionResult<OrderDto>> HandleAsync([FromRoute] string idOrOrderNumber, CancellationToken cancellationToken)
    {
        var getByIdResponse = await _orderService.GetAsync(idOrOrderNumber, cancellationToken);

        if (getByIdResponse.Status == ResponseStatus.Success)
        {
            return getByIdResponse.AsActionResult();
        }

        var getByOrderNumberResponse = await _orderService.GetByOrderNumberAsync(idOrOrderNumber, cancellationToken);
        return getByOrderNumberResponse.AsActionResult();
    }
}
