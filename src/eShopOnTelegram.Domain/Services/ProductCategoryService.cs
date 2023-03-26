using eShopOnTelegram.Domain.Extensions;
using eShopOnTelegram.Domain.Requests;
using eShopOnTelegram.Domain.Requests.ProductCategories;
using eShopOnTelegram.Domain.Responses.ProductCategories;
using eShopOnTelegram.Domain.Responses.Products;
using eShopOnTelegram.Domain.Services.Interfaces;

namespace eShopOnTelegram.Domain.Services;

public class ProductCategoryService : IProductCategoryService
{
    private readonly EShopOnTelegramDbContext _dbContext;
    private readonly ILogger<ProductCategoryService> _logger;

    public ProductCategoryService(EShopOnTelegramDbContext dbContext, ILogger<ProductCategoryService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Response<IEnumerable<GetProductCategoryResponse>>> GetMultipleAsync(GetRequest request, CancellationToken cancellationToken)
    {
        var response = new Response<IEnumerable<GetProductCategoryResponse>>();

        try
        {
            var products = await _dbContext.ProductCategories
                .WithPagination(request.PaginationModel)
                .ToListAsync(cancellationToken);

            var getProductCategoriesResponse = products.Select(product => new GetProductCategoryResponse
            {
                Name = product.Name,
            });

            response.Status = ResponseStatus.Success;
            response.Data = getProductCategoriesResponse;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception: Unable to get all products");
            response.Status = ResponseStatus.Exception;
        }

        return response;
    }

    public async Task<CreateResponse> CreateAsync(CreateProductCategoryRequest request, CancellationToken cancellationToken)
    {
        var response = new CreateResponse();

        try
        {
            var category = new ProductCategory()
            {
                Name = request.Name,
            };

            _dbContext.ProductCategories.Add(category);

            await _dbContext.SaveChangesAsync(cancellationToken);

            response.Status = ResponseStatus.Success;
            response.Id = category.Id;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception: Unable to create product category");
            response.Status = ResponseStatus.Exception;
        }

        return response;
    }

    public async Task<Response> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var response = new Response();

        try
        {
            var existingCategory = await _dbContext.ProductCategories.FirstOrDefaultAsync(category => category.Id == id);

            if (existingCategory == null)
            {
                response.Status = ResponseStatus.NotFound;
                return response;
            }

            _dbContext.ProductCategories.Remove(existingCategory);
            await _dbContext.SaveChangesAsync(cancellationToken);

            response.Status = ResponseStatus.Success;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception.Message);
            response.Status = ResponseStatus.Exception;
        }

        return response;
    }
}
