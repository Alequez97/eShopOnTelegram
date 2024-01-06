using eShopOnTelegram.Admin.Constants;
using eShopOnTelegram.Admin.Extensions;
using eShopOnTelegram.Domain.Dto.Orders;
using eShopOnTelegram.Domain.Requests;

using Microsoft.AspNetCore.Authorization;

namespace eShopOnTelegram.Admin.Endpoints.Orders;

public class GetOrders : EndpointBaseAsync
	.WithRequest<GetRequest>
	.WithActionResult<IEnumerable<OrderDto>>
{
	private readonly IOrderService _orderService;

	public GetOrders(IOrderService orderService)
	{
		_orderService = orderService;
	}

	[Authorize]
	[HttpGet("/api/orders")]
	[SwaggerOperation(Tags = new[] { SwaggerGroup.Orders })]
	public override async Task<ActionResult<IEnumerable<OrderDto>>> HandleAsync([FromQuery] GetRequest request, CancellationToken cancellationToken = default)
	{
		var response = await _orderService.GetMultipleAsync(request, cancellationToken);

		Response.AddPaginationHeaders(request.PaginationModel, response.TotalItemsInDatabase);

		return response.AsActionResult();
	}
}
