using eShopOnTelegram.Admin.Constants;
using eShopOnTelegram.Admin.Extensions;
using eShopOnTelegram.Domain.Dto.Orders;
using eShopOnTelegram.Domain.Responses;

using Microsoft.AspNetCore.Authorization;

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

	[Authorize]
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

		if (getByOrderNumberResponse.Status == ResponseStatus.ValidationFailed)
		{
			return new BadRequestResult();
		}

		if (getByOrderNumberResponse.Status == ResponseStatus.NotFound)
		{
			return new NotFoundResult();
		}

		if (getByOrderNumberResponse.Status == ResponseStatus.Exception)
		{
			return new StatusCodeResult(500);
		}

		// TODO: It is required for react admin id to be matched with requested id
		// Later should remove this temp fix with proper solution
		getByOrderNumberResponse.Data.Id = Convert.ToInt64(getByOrderNumberResponse.Data.OrderNumber);

		return new OkObjectResult(getByOrderNumberResponse.Data);
	}
}
