using eShopOnTelegram.Domain.Requests.Orders;
using eShopOnTelegram.Domain.Services.Interfaces;

namespace eShopOnTelegram.Domain.Services;

public class OrderService : IOrderService
{
    private readonly EShopOnTelegramDbContext _ctx;
    private readonly ILogger<OrderService> _logger;

    public OrderService(EShopOnTelegramDbContext ctx, ILogger<OrderService> logger)
    {
        _ctx = ctx;
        _logger = logger;
    }

    public async Task<Response> CreateOrder(CreateOrderRequest request)
    {
        try
        {
            var customer = await _ctx.Customers.FirstOrDefaultAsync(c => c.TelegramUserUID == request.TelegramUserUID);
            if (customer == null)
            {
                return new Response()
                {
                    Status = ResponseStatus.ValidationFailed,
                    Message = "User not found."
                };
            }

            // foreach cart item decrease quantity => Update product

            var order = new Order()
            {
                OrderNumber = Guid.NewGuid().ToString()[..18],
                CustomerId = customer.Id, // todo replace with real Id
                CartItems = request.CartItems,
                Status = OrderStatus.New
            };

            await _ctx.SaveChangesAsync();

            return new Response()
            {
                Status = ResponseStatus.Success,
                Message = $"Order {order.OrderNumber} created successfully!"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to create order.");
            return new Response()
            {
                Status = ResponseStatus.Exception
            };
        }
    }
}