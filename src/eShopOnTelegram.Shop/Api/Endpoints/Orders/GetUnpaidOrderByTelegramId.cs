using eShopOnTelegram.TelegramBot.Api.Attributes;
using eShopOnTelegram.TelegramBot.Api.Constants;

using Newtonsoft.Json;

namespace eShopOnTelegram.TelegramBot.Api.Endpoints.Orders;

public class GetUnpaidOrderByTelegramId : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult
{
    private readonly IOrderService _orderService;

    public GetUnpaidOrderByTelegramId(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [AuthorizeTelegram]
    [HttpGet("/api/orders/getUnpaidByTelegramId")]
    [SwaggerOperation(Tags = new[] { SwaggerGroup.Orders })]
    public override async Task<ActionResult> HandleAsync([FromQuery] string user, CancellationToken cancellationToken)
    {
        var telegramId = JsonConvert.DeserializeObject<dynamic>(user).id;
        var response = await _orderService.GetUnpaidOrderByTelegramIdAsync(telegramId, cancellationToken);

        return response.AsActionResult();
    }
}
