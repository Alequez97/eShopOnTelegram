using Bogus;

using eShopOnTelegram.Domain.Requests.Customers;
using eShopOnTelegram.Domain.Requests.Orders;
using eShopOnTelegram.Domain.Requests.ProductAttributes;
using eShopOnTelegram.Domain.Requests.ProductCategories;
using eShopOnTelegram.Domain.Requests.Products;
using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.Persistence.Context;
using eShopOnTelegram.Persistence.Entities;

using Microsoft.EntityFrameworkCore;

namespace eShopOnTelegram.Domain.IntegrationTests.Services.Orders;

public class CreateOrderTests
{
    private readonly IOrderService _orderService;
    private readonly ICustomerService _customerService;
    private readonly IProductService _productService;
    private readonly IProductCategoryService _productCategoryService;
    private readonly EShopOnTelegramDbContext _dbContext;
    private readonly Random _random = new(DateTime.Now.Millisecond);

    public CreateOrderTests(IOrderService orderService,
        ICustomerService customerService,
        IProductService productService,
        IProductCategoryService productCategoryService,
        EShopOnTelegramDbContext dbContext)
    {
        _orderService = orderService;
        _customerService = customerService;
        _productService = productService;
        _productCategoryService = productCategoryService;
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
                    ProductAttributeId = -1,
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

    [Fact]
    public async Task CreateOrder_WithValidCartItem_ProductItemsCountShouldBeDecreased()
    {
        // Arrange
        var existingCustomer = _dbContext.Customers.FirstOrDefault();
        if (existingCustomer == null)
        {
            await CreateCustomerAsync();
            existingCustomer = _dbContext.Customers.First();
        }

        var existingProductAttribute = _dbContext.ProductAttributes.FirstOrDefault(productAttribute => productAttribute.IsDeleted == false);
        if (existingProductAttribute == null)
        {
            await CreateProductAsync();
            existingProductAttribute = _dbContext.ProductAttributes.First();
        }

        var orderItemsAmount = 10;
        existingProductAttribute.DecreaseQuantity(orderItemsAmount);
        await _dbContext.SaveChangesAsync();

        var request = new CreateOrderRequest()
        {
            TelegramUserUID = existingCustomer.TelegramUserUID,
            CartItems = new List<CreateCartItemRequest>()
            {
                new CreateCartItemRequest()
                {
                    ProductAttributeId = existingProductAttribute.Id,
                    Quantity = orderItemsAmount
                }
            }
        };

        // Act
        var productLeftAmountBeforeOrder = existingProductAttribute.QuantityLeft;
        var response = await _orderService.CreateAsync(request, CancellationToken.None);

        // Assert
        response.Status.Should().Be(ResponseStatus.Success);

        var createdOrder = await _dbContext.Orders
            .Include(order => order.CartItems)
            .FirstOrDefaultAsync(order => order.Id == response.Id);

        createdOrder.Should().NotBeNull();
        createdOrder!.Status.Should().Be(OrderStatus.New);
        createdOrder!.CartItems.Count().Should().Be(1);
        createdOrder!.CartItems[0].ProductAttributeId.Should().Be(existingProductAttribute.Id);
        createdOrder!.CartItems[0].Quantity.Should().Be(orderItemsAmount);

        var orderedProductAttribute = await _dbContext.ProductAttributes.AsNoTracking().FirstAsync(product => product.Id == existingProductAttribute.Id);
        orderedProductAttribute.QuantityLeft.Should().Be(productLeftAmountBeforeOrder - orderItemsAmount);
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

    private async Task CreateProductAsync()
    {
        var existingProductCategory = _dbContext.ProductCategories.FirstOrDefault();
        if (existingProductCategory == null)
        {
            var createProductCategoryRequest = new Faker<CreateProductCategoryRequest>()
               .RuleFor(request => request.Name, faker => faker.Commerce.ProductAdjective())
               .Generate();

            var createProductCategoryResponse = await _productCategoryService.CreateAsync(createProductCategoryRequest, CancellationToken.None);
            createProductCategoryResponse.Status.Should().Be(ResponseStatus.Success);

            existingProductCategory = _dbContext.ProductCategories.First();
        }

        var createProductAttributeRequest = new Faker<CreateProductAttributeRequest>().Generate();

        var createProductRequest = new Faker<CreateProductRequest>()
                .RuleFor(request => request.Name, faker => faker.Commerce.ProductName())
                .RuleFor(request => request.ProductCategoryId, existingProductCategory.Id)
                .RuleFor(request => request.ProductAttributes, new List<CreateProductAttributeRequest>() { createProductAttributeRequest })
                .Generate();

        var response = await _productService.CreateAsync(createProductRequest, CancellationToken.None);

        response.Should().NotBeNull();
        response.Status.Should().Be(ResponseStatus.Success);
    }
}