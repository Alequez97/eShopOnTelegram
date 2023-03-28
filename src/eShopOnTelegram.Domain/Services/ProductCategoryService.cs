using eShopOnTelegram.Domain.Extensions;
using eShopOnTelegram.Domain.Requests;
using eShopOnTelegram.Domain.Requests.ProductCategories;
using eShopOnTelegram.Domain.Responses.ProductCategories;
using eShopOnTelegram.Domain.Services.Interfaces;

using Microsoft.Data.SqlClient;

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
            var productCategory = await _dbContext.ProductCategories
                .WithPagination(request.PaginationModel)
                .ToListAsync(cancellationToken);

            var getProductCategoriesResponse = productCategory.Select(productCategory => new GetProductCategoryResponse
            {
                Id = productCategory.Id,
                Name = productCategory.Name,
            });

            response.Status = ResponseStatus.Success;
            response.Data = getProductCategoriesResponse;
            response.TotalItemsInDatabase = await _dbContext.ProductCategories.CountAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception: Unable to get all products");
            response.Status = ResponseStatus.Exception;
        }

        return response;
    }

    public async Task<Response> CreateAsync(CreateProductCategoryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var category = new ProductCategory()
            {
                Name = request.Name,
            };

            _dbContext.ProductCategories.Add(category);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new Response()
            {
                Status = ResponseStatus.Success,
                Id = category.Id
            };
        }
        catch (DbUpdateException e)
            when (e?.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
        {
            return new Response()
            {
                Status = ResponseStatus.ValidationFailed
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception: Unable to create product category");
            return new Response()
            {
                Status = ResponseStatus.Exception
            };
        }
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
            response.Id = existingCategory.Id;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception.Message);
            response.Status = ResponseStatus.Exception;
        }

        return response;
    }
}
