using eShopOnTelegram.Domain.Requests;
using eShopOnTelegram.Domain.Requests.Orders;
using eShopOnTelegram.Domain.Responses.Orders;

namespace eShopOnTelegram.Domain.Services.Interfaces;

public interface IOrderService
{
    public Task<Response<IEnumerable<GetOrdersResponse>>> GetMultipleAsync(GetRequest request, CancellationToken cancellationToken);
    public Task<Response> CreateOrder(CreateOrderRequest request);
}
