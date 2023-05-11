using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.Persistence.Context;

namespace eShopOnTelegram.Domain.IntegrationTests.Services.Orders;

public class CreateOrderTests
{
    private readonly IOrderService _orderService;
    private readonly EShopOnTelegramDbContext _dbContext;

    public CreateOrderTests(IOrderService orderService, EShopOnTelegramDbContext dbContext)
    {
        _orderService = orderService;
        _dbContext = dbContext;
    }
}
