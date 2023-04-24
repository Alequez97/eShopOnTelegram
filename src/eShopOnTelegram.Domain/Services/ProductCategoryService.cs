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

    public async Task<Response<ProductCategoryDto>> GetAsync(long id, CancellationToken cancellationToken)
    {
        try
        {
            var existingProductCategory = await _dbContext.ProductCategories
                .FirstOrDefaultAsync(productCategory => productCategory.Id == id);

            if (existingProductCategory == null)
            {
                return new Response<ProductCategoryDto>
                {
                    Status = ResponseStatus.NotFound
                };
            }

            var getProductCategoryDto = new ProductCategoryDto()
            {
                Id = existingProductCategory.Id,
                Name = existingProductCategory.Name,
            };

            return new Response<ProductCategoryDto>()
            {
                Status = ResponseStatus.Success,
                Data = getProductCategoryDto,
            };

        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception: Unable to get all products");

            return new Response<ProductCategoryDto>()
            {
                Status = ResponseStatus.Exception
            };
        }
    }

    public async Task<Response<IEnumerable<ProductCategoryDto>>> GetMultipleAsync(GetRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var productCategory = await _dbContext.ProductCategories
                .Where(productCategory => productCategory.IsDeleted == false)
                .WithPagination(request.PaginationModel)
                .ToListAsync(cancellationToken);

            var getProductCategoriesDtoList = productCategory.Select(productCategory => new ProductCategoryDto
            {
                Id = productCategory.Id,
                Name = productCategory.Name,
            });

            return new Response<IEnumerable<ProductCategoryDto>>()
            {
                Status = ResponseStatus.Success,
                Data = getProductCategoriesDtoList,
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
                IsDeleted = false,
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

    public async Task<ActionResponse> UpdateAsync(UpdateProductCategoryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var existingProductCategory = _dbContext.ProductCategories.FirstOrDefault(category => category.Id == request.Id);

            if (existingProductCategory == null)
            {
                return new ActionResponse
                {
                    Status = ResponseStatus.NotFound
                };
            }

            existingProductCategory.Name = request.Name;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ActionResponse()
            {
                Id = existingProductCategory.Id,
                Status = ResponseStatus.Success
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception during product category update");

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

            existingCategory.IsDeleted = true;
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
