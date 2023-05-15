using Bogus;

using eShopOnTelegram.Domain.Requests.Customers;
using eShopOnTelegram.Domain.Requests.Orders;
using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.Persistence.Context;

namespace eShopOnTelegram.Domain.IntegrationTests.Services.Orders;

public class CreateOrderTests
{
    private readonly IOrderService _orderService;
    private readonly ICustomerService _customerService;
    private readonly EShopOnTelegramDbContext _dbContext;
    private readonly Random _random = new(DateTime.Now.Millisecond);

    public CreateOrderTests(IOrderService orderService, ICustomerService customerService, EShopOnTelegramDbContext dbContext)
    {
        _orderService = orderService;
        _customerService = customerService;
        _dbContext = dbContext;
    }

    [Fact]
    public async Task CreateOrder_WhenTelegramUserIdNotExists_ShouldReturnNotFound()
    {
        // Arrange 
        var request = new CreateOrderRequest()
        {
            TelegramUserUID = -1,
            CartItems = new List<CreateCartItemRequest>()
            {
                new CreateCartItemRequest()
                {
                    ProductId = -1,
                    Quantity = 5,
                }
            }
        };

        // Act
        var response = await _orderService.CreateAsync(request, CancellationToken.None);

        // Assert
        response.Status.Should().Be(ResponseStatus.NotFound);
    }

    [Fact]
    public async Task CreateOrder_WhenCartItemsIsEmpty_ShouldReturnValidationFailed()
    {
        // Arrange
        var existingCustomer = _dbContext.Customers.FirstOrDefault();
        if (existingCustomer == null)
        {
            await CreateCustomerAsync();
            existingCustomer = _dbContext.Customers.First();
        }

        var request = new CreateOrderRequest()
        {
            TelegramUserUID = existingCustomer.TelegramUserUID,
            CartItems = new List<CreateCartItemRequest>()
        };

        // Act
        var response = await _orderService.CreateAsync(request, CancellationToken.None);

        // Assert
        response.Status.Should().Be(ResponseStatus.ValidationFailed);
    }

    private async Task CreateCustomerAsync()
    {
        var createCustomerRequest = new Faker<CreateCustomerRequest>()
                .RuleFor(request => request.FirstName, faker => faker.Name.FirstName())
                .RuleFor(request => request.LastName, faker => faker.Name.LastName())
                .RuleFor(request => request.TelegramUserUID, _random.Next(1, 1000000000))
                .Generate();

        var response = await _customerService.CreateIfNotPresentAsync(createCustomerRequest);

        response.Should().NotBeNull();
        response.Status.Should().Be(ResponseStatus.Success);
    }
}