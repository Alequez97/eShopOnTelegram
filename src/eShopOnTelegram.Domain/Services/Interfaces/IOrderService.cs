using eShopOnTelegram.Domain.Dto.Orders;
using eShopOnTelegram.Domain.Requests;
using eShopOnTelegram.Domain.Requests.Orders;
using eShopOnTelegram.Domain.Responses.Orders;

namespace eShopOnTelegram.Domain.Services.Interfaces;

public interface IOrderService
{
    public Task<Response<OrderDto>> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken);

    public Task<Response<IEnumerable<OrderDto>>> GetByTelegramIdAsync(long telegramId, CancellationToken cancellationToken);

    public Task<Response<OrderDto>> GetUnpaidOrderByTelegramId(long telegramId, CancellationToken cancellationToken);

    public Task<Response<OrderDto>> GetUnpaidOrderByOrderNumber(string orderNumber, CancellationToken cancellationToken);

    public Task<Response<IEnumerable<OrderDto>>> GetMultipleAsync(GetRequest request, CancellationToken cancellationToken);

    public Task<CreateOrderResponse> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken);

    public Task<ActionResponse> UpdateStatusAsync(string orderNumber, OrderStatus orderStatus, CancellationToken cancellationToken);
}
