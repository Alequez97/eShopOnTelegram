using eShopOnTelegram.Domain.Dto.Orders;
using eShopOnTelegram.Domain.Extensions;
using eShopOnTelegram.Domain.Requests;
using eShopOnTelegram.Domain.Requests.Orders;
using eShopOnTelegram.Domain.Services.Interfaces;

namespace eShopOnTelegram.Domain.Services;

public class OrderService : IOrderService
{
    private readonly EShopOnTelegramDbContext _dbContext;
    private readonly ILogger<OrderService> _logger;

    public OrderService(EShopOnTelegramDbContext dbContext, ILogger<OrderService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
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

            var getOrdersResponse = orders.Select(order => new OrderDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                CustomerId = order.CustomerId,
                TelegramUserUID = order.Customer.TelegramUserUID,
                Username = order.Customer.Username,
                FirstName = order.Customer.FirstName,
                LastName = order.Customer.LastName,
                CartItems = order.CartItems.Select(cartItem => new CartItemDto()
                {
                    ProductId = cartItem.ProductId,
                    Name = cartItem.Product.Name,
                    CategoryName = cartItem.Product.Category.Name,
                    OriginalPrice = cartItem.Product.OriginalPrice,
                    PriceWithDiscount = cartItem.Product.PriceWithDiscount,
                    QuantityLeft = cartItem.Product.QuantityLeft, // probably we dont need this
                    ImageName = cartItem.Product.ImageName,
                    Quantity = cartItem.Quantity
                }).ToList(),
                CreationDate = order.CreationDate,
                PaymentDate = order.PaymentDate,
                Status = order.Status.ToString(),
                CountryIso2Code = order.CountryIso2Code,
                City = order.City,
                StreetLine1 = order.StreetLine1,
                StreetLine2 = order.StreetLine2,
                PostCode = order.PostCode,
            });


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

    public async Task<ActionResponse> CreateAsync(CreateOrderRequest request)
    {
        try
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.TelegramUserUID == request.TelegramUserUID);
            if (customer == null)
            {
                return new ActionResponse()
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

            _dbContext.Add(order);
            await _dbContext.SaveChangesAsync();

            return new ActionResponse()
            {
                Status = ResponseStatus.Success,
                Message = $"Order {order.OrderNumber} created successfully!"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to create order.");

            return new ActionResponse()
            {
                Status = ResponseStatus.Exception
            };
        }
    }
}