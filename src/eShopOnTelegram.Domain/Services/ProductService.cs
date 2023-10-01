﻿using eShopOnTelegram.Domain.Dto.Products;
using eShopOnTelegram.Domain.Extensions;
using eShopOnTelegram.Domain.Requests;
using eShopOnTelegram.Domain.Requests.Products;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.Persistence.Files.Interfaces;

using Microsoft.Extensions.Configuration;

namespace eShopOnTelegram.Domain.Services;

public class ProductService : IProductService
{
    private readonly EShopOnTelegramDbContext _dbContext;
    private readonly IProductImagesStore _productImagesStore;
    private readonly string _productImagesHostname;
    private readonly ILogger<ProductService> _logger;

    public ProductService(
        IDbContextFactory<EShopOnTelegramDbContext> dbContextFactory,
        IProductImagesStore productImagesStore,
        IConfiguration configuration,
        ILogger<ProductService> logger)
    {
        _dbContext = dbContextFactory.CreateDbContext();
        _productImagesStore = productImagesStore;
        _productImagesHostname = configuration["ProductImagesHostName"];
        _logger = logger;
    }

    public async Task<Response<IEnumerable<ProductDto>>> GetMultipleAsync(GetRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var products = await _dbContext.Products
                .Where(product => product.IsDeleted == false)
                .Include(product => product.Category)
                .Include(product => product.ProductAttributes)
                .WithPagination(request.PaginationModel)
            .ToListAsync(cancellationToken);

            var getProductsResponse = products.Select(product => product.ToProductDto(_productImagesHostname));

            return new Response<IEnumerable<ProductDto>>()
            {
                Status = ResponseStatus.Success,
                Data = getProductsResponse,
                TotalItemsInDatabase = await _dbContext.Products.Where(product => product.IsDeleted == false).CountAsync(cancellationToken)
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
                .Include(product => product.ProductAttributes)
                .Include(product => product.Category)
                .ToListAsync(cancellationToken);

            var getProductsResponse = products.Select(product => product.ToProductDto(_productImagesHostname));

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
                .Include(product => product.ProductAttributes)
                .Include(product => product.Category)
                .FirstOrDefaultAsync(product => product.Id == id && product.IsDeleted == false, cancellationToken);

            if (product == null)
            {
                return new Response<ProductDto>()
                {
                    Status = ResponseStatus.NotFound
                };
            }

            var getProductResponse = product.ToProductDto(_productImagesHostname);

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
                Name = request.Name,
                Category = existingProductCategory,
                IsDeleted = false,
            };

            var productAttributesList = new List<ProductAttribute>();
            foreach (var createProductAttributeRequest in request.ProductAttributes)
            {
                var storedImageName = await _productImagesStore.SaveAsync(createProductAttributeRequest.ProductImage, cancellationToken);

                var newProductAttribute = new ProductAttribute()
                {
                    Product = product,
                    Color = createProductAttributeRequest.Color,
                    Size = createProductAttributeRequest.Size,
                    OriginalPrice = createProductAttributeRequest.OriginalPrice,
                    PriceWithDiscount = createProductAttributeRequest.PriceWithDiscount,
                    QuantityLeft = createProductAttributeRequest.QuantityLeft,
                    IsDeleted = false,
                    ImageName = storedImageName,
                };

                productAttributesList.Add(newProductAttribute);
            }

            product.ProductAttributes = productAttributesList;

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

    // TODO: Fix update to update product attributes
    public async Task<ActionResponse> UpdateAsync(UpdateProductRequest updateProductRequest, CancellationToken cancellationToken)
    {
        try
        {
            var query = @"
                SELECT
                    P.*,
                    PC.Name AS ProductCategoryName,
                    PC.IsDeleted AS CategoryIsDeleted
                FROM Products as P WITH (UPDLOCK)
                INNER JOIN ProductCategories as PC on PC.Id = P.CategoryId
                WHERE P.Id = {0}";

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            var existingProduct = await _dbContext.Products.FromSqlRaw(query, updateProductRequest.Id).FirstOrDefaultAsync();

            if (existingProduct == null)
            {
                await transaction.RollbackAsync(cancellationToken);
                return new ActionResponse()
                {
                    Status = ResponseStatus.NotFound
                };
            }

            var updatedProduct = new Product()
            {
                Name = updateProductRequest.Name,
                CategoryId = existingProduct.CategoryId,
                //OriginalPrice = updateProductRequest.OriginalPrice,
                //PriceWithDiscount = updateProductRequest.PriceWithDiscount,
                //Quantity = updateProductRequest.Quantity,
                IsDeleted = false,
                PreviousVersion = existingProduct
            };

            existingProduct.IsDeleted = true;

            try
            {
                _dbContext.Products.Add(updatedProduct);
                await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update product when commiting transaction.");
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }

            //if (updateProductRequest.ProductImage != null)
            //{
            //    await _productImagesRepository.DeleteAsync(existingProduct.Image);
            //    var newImageName = await _productImagesRepository.SaveAsync(updateProductRequest.ProductImage, CancellationToken.None);
            //    existingProduct.Image = newImageName;
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

            foreach (var productAttribute in existingProduct.ProductAttributes)
            {
                productAttribute.IsDeleted = true;
                await _productImagesStore.DeleteAsync(productAttribute.ImageName);
            }

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