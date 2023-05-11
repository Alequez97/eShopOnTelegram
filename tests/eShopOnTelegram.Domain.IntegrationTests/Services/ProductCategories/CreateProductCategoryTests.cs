using Bogus;

using eShopOnTelegram.Domain.Requests.ProductCategories;
using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.Persistence.Context;

namespace eShopOnTelegram.Domain.IntegrationTests.Services.ProductCategories;

public class CreateProductCategoryTests
{
    private readonly IProductCategoryService _productCategoryService;
    private readonly EShopOnTelegramDbContext _dbContext;

    public CreateProductCategoryTests(IProductCategoryService productCategoryService, EShopOnTelegramDbContext dbContext)
    {
        _productCategoryService = productCategoryService;
        _dbContext = dbContext;
    }

    [Fact]
    public async Task CreateProductCategory_WhenRequestIsValid_ShouldReturnSuccess()
    {
        // Arrange 
        var request = new Faker<CreateProductCategoryRequest>()
            .RuleFor(request => request.Name, faker => faker.Commerce.Categories(1)[0])
            .Generate();

        // Act
        
        var response = await _productCategoryService.CreateAsync(request, CancellationToken.None);

        // Assert
        response.Status.Should().Be(ResponseStatus.Success);
    }
}
