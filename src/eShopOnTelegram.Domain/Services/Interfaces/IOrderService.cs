using eShopOnTelegram.Domain.Dto.Orders;
using eShopOnTelegram.Domain.Requests;
using eShopOnTelegram.Domain.Requests.Orders;

namespace eShopOnTelegram.Domain.Services.Interfaces;

public interface IOrderService
{
    public Task<Response<IEnumerable<OrderDto>>> GetMultipleAsync(GetRequest request, CancellationToken cancellationToken);
    public Task<ActionResponse> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken);
}
