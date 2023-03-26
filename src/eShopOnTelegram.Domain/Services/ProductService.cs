using eShopOnTelegram.Domain.Extensions;
using eShopOnTelegram.Domain.Requests;
using eShopOnTelegram.Domain.Requests.Products;
using eShopOnTelegram.Domain.Responses.Products;
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

    public async Task<Response<IEnumerable<GetProductResponse>>> GetMultipleAsync(GetRequest request, CancellationToken cancellationToken)
    {
        var response = new Response<IEnumerable<GetProductResponse>>();

        try
        {
            var products = await _dbContext.Products
                .Include(product => product.Category)
                .WithPagination(request.PaginationModel)
                .ToListAsync(cancellationToken);

            var getProductsResponse = products.Select(product => new GetProductResponse
            {
                Id = product.Id,
                ProductName = product.Name,
                ProductCategoryName = product.Category.Name,
                OriginalPrice = product.OriginalPrice,
                PriceWithDiscount = product.PriceWithDiscount,
                QuantityLeft = product.QuantityLeft,
                //Image = product.ImageName
            });

            response.Status = ResponseStatus.Success;
            response.Data = getProductsResponse;
            response.TotalItemsInDatabase = await _dbContext.Products.CountAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception: Unable to get all products");
            response.Status = ResponseStatus.Exception;
        }

        return response;
    }

    public async Task<Response<GetProductResponse>> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var response = new Response<GetProductResponse>();

        try
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken);

            if (product == null)
            {
                response.Status = ResponseStatus.NotFound;
                return response;
            }

            var getProductResponse = new GetProductResponse()
            {
                Id = product.Id,
                ProductName = product.Name,
                ProductCategoryName = product.Category.Name,
                OriginalPrice = product.OriginalPrice,
                PriceWithDiscount = product.PriceWithDiscount,
                QuantityLeft = product.QuantityLeft,
                Image = product.ImageName
            };

            response.Status = ResponseStatus.Success;
            response.Data = getProductResponse;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception: Unable to get product with id - {Id}", id);
            response.Status = ResponseStatus.Exception;
        }

        return response;
    }

    public async Task<Response> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var response = new Response();

        try
        {
            var existingProductCategory = await _dbContext.ProductCategories
                .FirstOrDefaultAsync(category => category.Id == request.ProductCategoryId, cancellationToken);

            if (existingProductCategory == null)
            {
                response.Status = ResponseStatus.NotFound;
                return response;
            }

            var product = new Product()
            {
                Name = request.ProductName,
                OriginalPrice = request.OriginalPrice,
                PriceWithDiscount = request.PriceWithDiscount,
                QuantityLeft = request.QuantityLeft,
                Category = existingProductCategory
            };

            var productCategory = await _dbContext.ProductCategories
                .FirstOrDefaultAsync(productCategory => productCategory.Id == request.ProductCategoryId, cancellationToken);

            if (productCategory == null)
            {
                response.Status = ResponseStatus.NotFound;
            }

            _dbContext.Products.Add(product);

            await _dbContext.SaveChangesAsync(cancellationToken);

            response.Status = ResponseStatus.Success;
            response.Id = product.Id;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception: Unable to create product");
            response.Status = ResponseStatus.Exception;
        }

        return response;
    }

    public async Task<Response> UpdateAsync(UpdateProductRequest updateProductRequest, CancellationToken cancellationToken)
    {
        var response = new Response();

        try
        {
            var existingProduct = await _dbContext.Products.FirstOrDefaultAsync(product => product.Id == updateProductRequest.Id);

            if (existingProduct == null)
            {
                response.Status = ResponseStatus.NotFound;
                return response;
            }

            existingProduct.Name = updateProductRequest.ProductName;
            existingProduct.OriginalPrice = updateProductRequest.OriginalPrice;
            existingProduct.PriceWithDiscount = updateProductRequest.PriceWithDiscount;
            existingProduct.QuantityLeft = updateProductRequest.QuantityLeft;

            //if (updateProductRequest.ProductImage != null)
            //{
            //    await _productImagesRepository.DeleteAsync(existingProduct.ImageName);
            //    var newImageName = await _productImagesRepository.SaveAsync(updateProductRequest.ProductImage, CancellationToken.None);
            //    existingProduct.ImageName = newImageName;
            //}

            await _dbContext.SaveChangesAsync(cancellationToken);

            response.Status = ResponseStatus.Success;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception: Unable to update product with id - {Id}", updateProductRequest.Id);
            response.Status = ResponseStatus.Exception;
        }

        return response;
    }

    public async Task<Response> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var response = new Response();

        try
        {
            var existingProduct = await _dbContext.Products.FirstOrDefaultAsync(product => product.Id == id);

            if (existingProduct == null)
            {
                response.Status = ResponseStatus.NotFound;
                return response;
            }

            _dbContext.Products.Remove(existingProduct);
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