using eShopOnTelegram.Domain.Dto.Orders;
using eShopOnTelegram.Domain.Extensions;
using eShopOnTelegram.Domain.Requests;
using eShopOnTelegram.Domain.Requests.Orders;
using eShopOnTelegram.Domain.Responses.Orders;
using eShopOnTelegram.Domain.Services.Interfaces;

namespace eShopOnTelegram.Domain.Services;

public class OrderService : IOrderService
{
    private readonly EShopOnTelegramDbContext _dbContext;
    private readonly ILogger<OrderService> _logger;
    private readonly Random _random = new(DateTime.Now.Millisecond);

    public OrderService(EShopOnTelegramDbContext dbContext, ILogger<OrderService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Response<OrderDto>> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken)
    {
        try
        {
            var existingOrder = await _dbContext.Orders.FirstOrDefaultAsync(order => order.OrderNumber == orderNumber, cancellationToken);

            if (existingOrder == null)
            {
                return new Response<OrderDto>()
                {
                    Status = ResponseStatus.NotFound
                };
            }

            return new Response<OrderDto>
            {
                Status = ResponseStatus.Success,
                Data = existingOrder.ToOrderDto()
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            return new Response<OrderDto>()
            {
                Status = ResponseStatus.Exception
            };
        }
    }

    public async Task<Response<IEnumerable<OrderDto>>> GetByTelegramIdAsync(long telegramId, CancellationToken cancellationToken)
    {
        try
        {
            var customerOrders = _dbContext.Orders
                .Include(order => order.CartItems)
                .ThenInclude(cartItem => cartItem.Product)
                .ThenInclude(product => product.Category)
                .Include(order => order.Customer)
                .Where(order => order.Customer.TelegramUserUID == telegramId);

            if (!customerOrders.Any())
            {
                return new Response<IEnumerable<OrderDto>>()
                {
                    Status = ResponseStatus.NotFound
                };
            }

            return await Task.FromResult(new Response<IEnumerable<OrderDto>>()
            {
                Status = ResponseStatus.Success,
                Data = customerOrders.Select(order => order.ToOrderDto())
            });
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            return new Response<IEnumerable<OrderDto>>()
            {
                Status = ResponseStatus.Exception
            };
        }
    }

    public async Task<Response<IEnumerable<OrderDto>>> GetMultipleAsync(GetRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var orders = await _dbContext.Orders
                .Include(order => order.CartItems)
                .ThenInclude(cartItem => cartItem.Product)
                .ThenInclude(product => product.Category)
                .Include(order => order.Customer)
                .WithPagination(request.PaginationModel)
                .ToListAsync(cancellationToken);

            var getOrdersResponse = orders.Select(order => order.ToOrderDto());


            return new Response<IEnumerable<OrderDto>>()
            {
                Status = ResponseStatus.Success,
                Data = getOrdersResponse,
                TotalItemsInDatabase = await _dbContext.Orders.CountAsync(cancellationToken)
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception: Unable to get all products");

            return new Response<IEnumerable<OrderDto>>()
            {
                Status = ResponseStatus.Exception
            };
        }
    }

    public async Task<CreateOrderResponse> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.TelegramUserUID == request.TelegramUserUID);
            if (customer == null)
            {
                return new CreateOrderResponse()
                {
                    Status = ResponseStatus.NotFound,
                };
            }

            if (request.CartItems.Count == 0)
            {
                return new CreateOrderResponse()
                {
                    Status = ResponseStatus.ValidationFailed,
                    Message = "Cart items cannot be empty during order creation"
                };
            }

            foreach (var requestCartItem in request.CartItems)
            {
                var product = _dbContext.Products.FirstOrDefault(product => product.Id == requestCartItem.ProductId);

                if (product == null)
                {
                    return new CreateOrderResponse()
                    {
                        Status = ResponseStatus.NotFound,
                        Message = $"Product with id {requestCartItem.ProductId} not found"
                    };
                }

                if (product.IsDeleted == true)
                {
                    return new CreateOrderResponse()
                    {
                        Status = ResponseStatus.Exception,
                        Message = $"Product was updated while request was processed"
                    };
                }

                if (product.QuantityLeft < requestCartItem.Quantity)
                {
                    return new CreateOrderResponse()
                    {
                        Status = ResponseStatus.ValidationFailed,
                        Message = $"Requested {requestCartItem.Quantity} amount of product with id {requestCartItem.ProductId}, but only {product.QuantityLeft} is available"
                    };
                }

                product.QuantityLeft -= requestCartItem.Quantity;
            }

            var orderCartItems = request.CartItems.Select(requestedCartItem => new CartItem()
            {
                ProductId = requestedCartItem.ProductId,
                Quantity = requestedCartItem.Quantity
            }).ToList();

            var order = new Order()
            {
                OrderNumber = GenerateOrderNumber(),
                CustomerId = customer.Id,
                CartItems = orderCartItems,
                Status = OrderStatus.New
            };

            _dbContext.Add(order);
            await _dbContext.SaveChangesAsync();

            return new CreateOrderResponse()
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                Status = ResponseStatus.Success,
                Message = $"Order {order.OrderNumber} created successfully!"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to create order.");

            return new CreateOrderResponse()
            {
                Status = ResponseStatus.Exception
            };
        }
    }

    public async Task<ActionResponse> UpdateStatusAsync(string orderNumber, OrderStatus orderStatus, CancellationToken cancellationToken)
    {
        try
        {
            var existingOrder = await _dbContext.Orders.FirstOrDefaultAsync(order => order.OrderNumber == orderNumber, cancellationToken);

            if (existingOrder == null)
            {
                return new ActionResponse()
                {
                    Status = ResponseStatus.NotFound
                };
            }

            existingOrder.Status = orderStatus;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ActionResponse
            { 
                Status = ResponseStatus.Success
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            return new ActionResponse()
            {
                Status = ResponseStatus.Exception
            };
        }
    }

    private string GenerateOrderNumber()
    {
        return $"{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}{_random.Next(1000, 9999)}";
    }
}