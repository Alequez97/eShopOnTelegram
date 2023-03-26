using eShopOnTelegram.Domain.Requests.Orders;

namespace eShopOnTelegram.Domain.Services.Interfaces;

public interface IOrderService
{
    public Task<Response> CreateOrder(CreateOrderRequest request);
}
