using eShopOnTelegram.Admin.Constants;
using eShopOnTelegram.Admin.Extensions;
using eShopOnTelegram.Domain.Dto.Orders;
using eShopOnTelegram.Domain.Responses;

namespace eShopOnTelegram.Admin.Endpoints.Orders;

public class GetOrderByOrderNumber : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<OrderDto>
{
    private readonly IOrderService _orderService;

    public GetOrderByOrderNumber(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("/api/orders/{orderNumber}")]
    [SwaggerOperation(Tags = new[] { SwaggerGroup.Orders })]
    public override async Task<ActionResult<OrderDto>> HandleAsync([FromRoute] string orderNumber, CancellationToken cancellationToken)
    {
        var response = await _orderService.GetByOrderNumberAsync(orderNumber, cancellationToken);
        
        if (response.Status == ResponseStatus.ValidationFailed)
        {
            return new BadRequestResult();
        }

        if (response.Status == ResponseStatus.NotFound)
        {
            return new NotFoundResult();
        }

        if (response.Status == ResponseStatus.Exception)
        {
            return new StatusCodeResult(500);
        }

        // TODO: For react-admin it is required for react admin to id be match with requested id
        // Remove this temp fix with proper solution
        response.Data.Id = Convert.ToInt64(response.Data.OrderNumber);

        return new OkObjectResult(response.Data);

        //return response.AsActionResult()
    }
}
