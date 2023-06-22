using eShopOnTelegram.Domain.Dto.Orders;

namespace eShopOnTelegram.Domain.Responses.Orders;

public class CreateOrderResponse : ActionResponse
{
    public OrderDto? CreatedOrder { get; set; }
}
