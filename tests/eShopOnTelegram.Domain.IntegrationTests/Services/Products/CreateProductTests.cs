using eShopOnTelegram.Domain.Requests.ProductAttributes;
using eShopOnTelegram.Domain.Requests.Products;
using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.Persistence.Context;

namespace eShopOnTelegram.Domain.IntegrationTests.Services.Products;

public class CreateProductTests
{
    private readonly IProductService _productService;
    private readonly EShopOnTelegramDbContext _dbContext;

    public CreateProductTests(IProductService productService, EShopOnTelegramDbContext dbContext)
    {
        _productService = productService;
        _dbContext = dbContext;
    }

    [Fact]
    public async Task CreateProduct_WhenProductCategoryNotExists_ShouldReturnNotFound()
    {
        // Arrange 
        var request = new CreateProductRequest()
        {
            Name = "Test-Name",
            ProductCategoryId = -1,
            ProductAttributes = new List<CreateProductAttributeRequest>()
        };

        // Act
        var response = await _productService.CreateAsync(request, CancellationToken.None);

        // Assert
        response.Status.Should().Be(ResponseStatus.NotFound);
    }
}
