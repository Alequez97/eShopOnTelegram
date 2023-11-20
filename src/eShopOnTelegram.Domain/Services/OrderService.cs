using eShopOnTelegram.Domain.Dto.Orders;
using eShopOnTelegram.Domain.Extensions;
using eShopOnTelegram.Domain.Requests;
using eShopOnTelegram.Domain.Requests.Orders;
using eShopOnTelegram.Domain.Responses.Orders;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.Persistence.Entities.Orders;

namespace eShopOnTelegram.Domain.Services;

public class OrderService : IOrderService
{
    private readonly EShopOnTelegramDbContext _dbContext;
    private readonly ILogger<OrderService> _logger;
    private readonly Random _random = new(Guid.NewGuid().GetHashCode());

    public OrderService(IDbContextFactory<EShopOnTelegramDbContext> dbContextFactory, ILogger<OrderService> logger)
    {
        _dbContext = dbContextFactory.CreateDbContext();
        _logger = logger;
    }

    public async Task<Response<OrderDto>> GetAsync(string id, CancellationToken cancellationToken)
    {
        try
        {
            var existingOrder = await _dbContext.Orders
                .Include(order => order.CartItems)
                .ThenInclude(cartItem => cartItem.ProductAttribute)
                .ThenInclude(productAttribute => productAttribute.Product)
                .ThenInclude(product => product.Category)
                .Include(order => order.Customer)
                .FirstOrDefaultAsync(order => order.Id == Convert.ToInt64(id), cancellationToken);

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

    public async Task<Response<OrderDto>> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken)
    {
        try
        {
            var existingOrder = await _dbContext.Orders
                .Include(order => order.CartItems)
                .ThenInclude(cartItem => cartItem.ProductAttribute)
                .ThenInclude(productAttribute => productAttribute.Product)
                .ThenInclude(product => product.Category)
                .Include(order => order.Customer)
                .FirstOrDefaultAsync(order => order.OrderNumber == orderNumber, cancellationToken);

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
                .ThenInclude(cartItem => cartItem.ProductAttribute)
                .ThenInclude(productAttribute => productAttribute.Product)
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

    public async Task<Response<OrderDto>> GetUnpaidOrderByTelegramIdAsync(long telegramId, CancellationToken cancellationToken)
    {
        try
        {
            var customerOrders = await _dbContext.Orders
                .Include(order => order.CartItems)
                .ThenInclude(cartItem => cartItem.ProductAttribute)
                .ThenInclude(productAttribute => productAttribute.Product)
                .ThenInclude(product => product.Category)
                .Include(order => order.Customer)
                .Where(order => order.Customer.TelegramUserUID == telegramId)
                .Where(order => order.Status == OrderStatus.New || order.Status == OrderStatus.AwaitingPayment)
                //.Where(order => order.Status == OrderStatus.New || order.Status == OrderStatus.InvoiceSent)
                .ToListAsync(cancellationToken);

            if (!customerOrders.Any())
            {
                return new Response<OrderDto>()
                {
                    Status = ResponseStatus.NotFound
                };
            }

            if (customerOrders.Count > 1)
            {
                return new Response<OrderDto>()
                {
                    Status = ResponseStatus.ValidationFailed,
                    Message = "Error. For every customer should be only one order with status new"
                };
            }

            return new Response<OrderDto>()
            {
                Status = ResponseStatus.Success,
                Data = customerOrders.First().ToOrderDto()
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

    public async Task<Response<OrderDto>> GetUnpaidOrderByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken)
    {
        try
        {
            var customerOrders = await _dbContext.Orders
                .Include(order => order.CartItems)
                .ThenInclude(cartItem => cartItem.ProductAttribute)
                .ThenInclude(productAttribute => productAttribute.Product)
                .ThenInclude(product => product.Category)
                .Include(order => order.Customer)
                .Where(order => order.OrderNumber == orderNumber)
                .Where(order => order.Status == OrderStatus.New || order.Status == OrderStatus.AwaitingPayment)
                //.Where(order => order.Status == OrderStatus.New || order.Status == OrderStatus.InvoiceSent)
                .ToListAsync(cancellationToken);

            if (!customerOrders.Any())
            {
                return new Response<OrderDto>()
                {
                    Status = ResponseStatus.NotFound
                };
            }

            if (customerOrders.Count > 1)
            {
                return new Response<OrderDto>()
                {
                    Status = ResponseStatus.ValidationFailed,
                    Message = "Error. For every customer should be only one order with status new. Found more than one"
                };
            }

            return new Response<OrderDto>()
            {
                Status = ResponseStatus.Success,
                Data = customerOrders.First().ToOrderDto()
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

    public async Task<Response<IEnumerable<OrderDto>>> GetMultipleAsync(GetRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var orders = await _dbContext.Orders
                .Include(order => order.CartItems)
                .ThenInclude(cartItem => cartItem.ProductAttribute)
                .ThenInclude(productAttribute => productAttribute.Product)
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

            var getUnpdaidOrderResponse = await GetUnpaidOrderByTelegramIdAsync(customer.TelegramUserUID, cancellationToken);
            if (getUnpdaidOrderResponse.Status == ResponseStatus.Success || getUnpdaidOrderResponse.Data != null)
            {
                var response = await DeleteByOrderNumberAsync(getUnpdaidOrderResponse.Data.OrderNumber, cancellationToken);

                if (response.Status != ResponseStatus.Success)
                {
                    return new CreateOrderResponse()
                    {
                        Status = ResponseStatus.Exception,
                    };
                }
            }

            if (request.CartItems.Count == 0)
            {
                return new CreateOrderResponse()
                {
                    Status = ResponseStatus.ValidationFailed,
                    Message = "Cart items cannot be empty during order creation"
                };
            }

            using var transaction = _dbContext.Database.BeginTransaction(); // Default Transaction Isolation Level is ReadCommited

            var productAttributesIds = string.Join(",", request.CartItems.Select(cartItem => cartItem.ProductAttributeId).ToArray());
            var query = $"SELECT * FROM ProductAttributes WITH (UPDLOCK) WHERE Id IN ({productAttributesIds}) AND IsDeleted = 0"; // Fetch requested products with UPDLOCK

            var requestedProductAttributes = await _dbContext.ProductAttributes.FromSqlRaw(query).ToListAsync(cancellationToken);

            if (request.CartItems.Count != requestedProductAttributes.Count) // Either deleted or does not exist
            {
                await transaction.RollbackAsync();
                return new CreateOrderResponse()
                {
                    Status = ResponseStatus.NotFound,
                    Message = "Some of requested products was not found"
                };
            }

            foreach (var productAttribute in requestedProductAttributes) // todo could be optimized probably
            {
                var requestedCountToBuy = request.CartItems.Where(ci => ci.ProductAttributeId == productAttribute.Id).Select(ci => ci.Quantity).First();
                if (productAttribute.QuantityLeft < requestedCountToBuy)
                {
                    await transaction.RollbackAsync();
                    return new CreateOrderResponse()
                    {
                        Status = ResponseStatus.ValidationFailed,
                        Message = $"Requested {requestedCountToBuy} amount of productAttribute with id {productAttribute.Id}, but only {productAttribute.QuantityLeft} is available"
                    };
                }
                productAttribute.DecreaseQuantity(requestedCountToBuy);
            }

            var orderCartItems = request.CartItems.Select(requestedCartItem => new CartItem()
            {
                ProductAttributeId = requestedCartItem.ProductAttributeId,
                Quantity = requestedCartItem.Quantity
            }).ToList();

            var order = new Order()
            {
                OrderNumber = GenerateOrderNumber(),
                CustomerId = customer.Id,
                CartItems = orderCartItems,
                Status = OrderStatus.New,
                PaymentStatus = OrderPaymentStatus.None,
                PaymentMethod = OrderPaymentMethod.None
            };

            try
            {
                _dbContext.Add(order);
                await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to make products update while creating order");
                transaction.Rollback();
                throw;
            }

            var createdOrder = await GetByOrderNumberAsync(order.OrderNumber, cancellationToken);

            return new CreateOrderResponse()
            {
                Id = order.Id,
                CreatedOrder = createdOrder.Data,
                Status = ResponseStatus.Success,
                Message = $"Order {order.OrderNumber} created successfully!"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to create order.");

            return new CreateOrderResponse()
            {
                Status = ResponseStatus.Exception,
                Message = $"{ex.Message} {(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}"
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
            if (orderStatus == OrderStatus.Paid)
            {
                existingOrder.PaymentDate = DateTime.UtcNow;
            }

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

    public async Task<ActionResponse> DeleteByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken)
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

            _dbContext.Orders.Remove(existingOrder);
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