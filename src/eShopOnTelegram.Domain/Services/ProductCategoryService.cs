using eShopOnTelegram.Domain.Dto.ProductsCategories;
using eShopOnTelegram.Domain.Extensions;
using eShopOnTelegram.Domain.Requests;
using eShopOnTelegram.Domain.Requests.ProductCategories;
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

    public async Task<Response<IEnumerable<ProductCategoryDto>>> GetMultipleAsync(GetRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var productCategory = await _dbContext.ProductCategories
                .WithPagination(request.PaginationModel)
                .ToListAsync(cancellationToken);

            var getProductCategoriesResponse = productCategory.Select(productCategory => new ProductCategoryDto
            {
                Id = productCategory.Id,
                Name = productCategory.Name,
            });

            return new Response<IEnumerable<ProductCategoryDto>>()
            {
                Status = ResponseStatus.Success,
                Data = getProductCategoriesResponse,
                TotalItemsInDatabase = await _dbContext.ProductCategories.CountAsync(cancellationToken)
            };

        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception: Unable to get all products");

            return new Response<IEnumerable<ProductCategoryDto>>()
            {
                Status = ResponseStatus.Exception
            };
        }
    }

    public async Task<ActionResponse> CreateAsync(CreateProductCategoryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var category = new ProductCategory()
            {
                Name = request.Name,
            };

            _dbContext.ProductCategories.Add(category);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ActionResponse()
            {
                Status = ResponseStatus.Success,
                Id = category.Id
            };
        }
        catch (DbUpdateException e)
            when (e?.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
        {
            return new ActionResponse()
            {
                Status = ResponseStatus.ValidationFailed
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception: Unable to create product category");

            return new ActionResponse()
            {
                Status = ResponseStatus.Exception
            };
        }
    }

    public async Task<ActionResponse> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        try
        {
            var existingCategory = await _dbContext.ProductCategories.FirstOrDefaultAsync(category => category.Id == id);

            if (existingCategory == null)
            {
                return new ActionResponse()
                {
                    Status = ResponseStatus.NotFound
                };
            }

            _dbContext.ProductCategories.Remove(existingCategory);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ActionResponse()
            {
                Status = ResponseStatus.Success,
                Id = existingCategory.Id
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception.Message);

            return new ActionResponse()
            {
                Status = ResponseStatus.Exception
            };
        }
    }
}
