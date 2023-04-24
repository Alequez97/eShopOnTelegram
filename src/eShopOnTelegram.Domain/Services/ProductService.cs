using eShopOnTelegram.Domain.Dto.Products;
using eShopOnTelegram.Domain.Extensions;
using eShopOnTelegram.Domain.Requests;
using eShopOnTelegram.Domain.Requests.Products;
using eShopOnTelegram.Domain.Services.Interfaces;

namespace eShopOnTelegram.Domain.Services;

public class ProductService : IProductService
{
    private readonly EShopOnTelegramDbContext _dbContext;
    private readonly ILogger<ProductService> _logger;

    public ProductService(EShopOnTelegramDbContext dbContext, ILogger<ProductService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Response<IEnumerable<ProductDto>>> GetMultipleAsync(GetRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var products = await _dbContext.Products
                .Where(product => product.IsDeleted == false)
                .Include(product => product.Category)
                .WithPagination(request.PaginationModel)
                .ToListAsync(cancellationToken);

            var getProductsResponse = products.Select(product => new ProductDto
            {
                Id = product.Id,
                ProductName = product.Name,
                ProductCategoryName = product.Category.Name,
                OriginalPrice = product.OriginalPrice,
                PriceWithDiscount = product.PriceWithDiscount,
                QuantityLeft = product.QuantityLeft,
                //Image = product.ImageName
            });

            return new Response<IEnumerable<ProductDto>>()
            {
                Status = ResponseStatus.Success,
                Data = getProductsResponse,
                TotalItemsInDatabase = await _dbContext.Products.CountAsync(cancellationToken)
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception: Unable to get all products");

            return new Response<IEnumerable<ProductDto>>()
            {
                Status = ResponseStatus.Exception
            };
        }
    }

    public async Task<Response<IEnumerable<ProductDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        try
        {
            var products = await _dbContext.Products
                .Where(product => product.IsDeleted == false)
                .Include(product => product.Category)
                .ToListAsync(cancellationToken);

            var getProductsResponse = products.Select(product => new ProductDto
            {
                Id = product.Id,
                ProductName = product.Name,
                ProductCategoryName = product.Category.Name,
                OriginalPrice = product.OriginalPrice,
                PriceWithDiscount = product.PriceWithDiscount,
                QuantityLeft = product.QuantityLeft,
                //Image = product.ImageName
            });

            return new Response<IEnumerable<ProductDto>>()
            {
                Status = ResponseStatus.Success,
                Data = getProductsResponse,
                TotalItemsInDatabase = await _dbContext.Products.CountAsync(cancellationToken)
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception: Unable to get all products");
            return new Response<IEnumerable<ProductDto>>()
            {
                Status = ResponseStatus.Exception
            };
        }
    }

    public async Task<Response<ProductDto>> GetAsync(long id, CancellationToken cancellationToken)
    {
        try
        {
            var product = await _dbContext.Products
                .Include(product => product.Category)
                .FirstOrDefaultAsync(product => product.Id == id && product.IsDeleted == false, cancellationToken);

            if (product == null)
            {
                return new Response<ProductDto>()
                {
                    Status = ResponseStatus.NotFound
                };
            }

            var getProductResponse = new ProductDto()
            {
                Id = product.Id,
                ProductName = product.Name,
                ProductCategoryName = product.Category.Name,
                OriginalPrice = product.OriginalPrice,
                PriceWithDiscount = product.PriceWithDiscount,
                QuantityLeft = product.QuantityLeft,
                Image = product.ImageName
            };

            return new Response<ProductDto>()
            {
                Status = ResponseStatus.Success,
                Data = getProductResponse
            };

        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception: Unable to get product with id - {Id}", id);

            return new Response<ProductDto>()
            {
                Status = ResponseStatus.Exception
            };
        }
    }

    public async Task<ActionResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var existingProductCategory = await _dbContext.ProductCategories
                .FirstOrDefaultAsync(category => category.Id == request.ProductCategoryId, cancellationToken);

            if (existingProductCategory == null)
            {
                return new ActionResponse()
                {
                    Status = ResponseStatus.NotFound
                };
            }

            var product = new Product()
            {
                Name = request.ProductName,
                OriginalPrice = request.OriginalPrice,
                PriceWithDiscount = request.PriceWithDiscount,
                QuantityLeft = request.QuantityLeft,
                Category = existingProductCategory,
                IsDeleted = false
            };

            _dbContext.Products.Add(product);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ActionResponse()
            {
                Status = ResponseStatus.Success,
                Id = product.Id
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception: Unable to create product");

            return new ActionResponse()
            {
                Status = ResponseStatus.Exception
            };
        }
    }

    public async Task<ActionResponse> UpdateAsync(UpdateProductRequest updateProductRequest, CancellationToken cancellationToken)
    {
        try
        {
            var existingProduct = await _dbContext.Products.FirstOrDefaultAsync(product => product.Id == updateProductRequest.Id);

            if (existingProduct == null)
            {
                return new ActionResponse()
                {
                    Status = ResponseStatus.NotFound
                };
            }

            var updatedProduct = new Product()
            {
                Name = updateProductRequest.ProductName,
                Category = existingProduct.Category,
                OriginalPrice = updateProductRequest.OriginalPrice,
                PriceWithDiscount = updateProductRequest.PriceWithDiscount,
                QuantityLeft = updateProductRequest.QuantityLeft,
                IsDeleted = false,
                PreviousVersion = existingProduct
            };

            existingProduct.IsDeleted = true;

            _dbContext.Products.Add(updatedProduct);
            await _dbContext.SaveChangesAsync(cancellationToken);

            //if (updateProductRequest.ProductImage != null)
            //{
            //    await _productImagesRepository.DeleteAsync(existingProduct.ImageName);
            //    var newImageName = await _productImagesRepository.SaveAsync(updateProductRequest.ProductImage, CancellationToken.None);
            //    existingProduct.ImageName = newImageName;
            //}

            return new ActionResponse()
            {
                Status = ResponseStatus.Success
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception: Unable to update product with id - {Id}", updateProductRequest.Id);

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
            var existingProduct = await _dbContext.Products.FirstOrDefaultAsync(product => product.Id == id);

            if (existingProduct == null)
            {
                return new ActionResponse()
                {
                    Status = ResponseStatus.NotFound
                };
            }

            existingProduct.IsDeleted = true;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ActionResponse()
            {
                Status = ResponseStatus.Success
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