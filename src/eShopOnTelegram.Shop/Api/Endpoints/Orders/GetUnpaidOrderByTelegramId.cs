using eShopOnTelegram.TelegramBot.Api.Constants;
using eShopOnTelegram.TelegramBot.Api.Extensions;

namespace eShopOnTelegram.TelegramBot.Api.Endpoints.Orders;

public class GetUnpaidOrderByTelegramId : EndpointBaseAsync
    .WithRequest<long>
    .WithActionResult
{
    private readonly IOrderService _orderService;

    public GetUnpaidOrderByTelegramId(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("/api/orders/getUnpaidByTelegramId/{telegramId}")]
    [SwaggerOperation(Tags = new[] { SwaggerGroup.Orders })]
    public override async Task<ActionResult> HandleAsync([FromRoute] long telegramId, CancellationToken cancellationToken)
    {
        var response = await _orderService.GetUnpaidOrderByTelegramIdAsync(telegramId, cancellationToken);

        return response.AsActionResult();
    }
}
