﻿using eShopOnTelegram.Domain.Dto.ProductAttributes;
using eShopOnTelegram.Domain.Requests.ProductAttributes;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.Utils.Configuration;

namespace eShopOnTelegram.Domain.Services;

public class ProductAttributeService : IProductAttributeService
{
	private readonly EShopOnTelegramDbContext _dbContext;
	private readonly string _productImagesHostname;
	private readonly ILogger<ProductAttributeService> _logger;

	public ProductAttributeService(
		IDbContextFactory<EShopOnTelegramDbContext> dbContextFactory,
		AppSettings appSettings,
		ILogger<ProductAttributeService> logger)
	{
		_dbContext = dbContextFactory.CreateDbContext();
		_productImagesHostname = appSettings.ProductImagesHostName;
		_logger = logger;
	}

	public async Task<Response<ProductAttributeDto>> GetAsync(long id, CancellationToken cancellationToken)
	{
		try
		{
			var productAttribute = await _dbContext.ProductAttributes
				.Include(productAttribute => productAttribute.Product)
				.ThenInclude(product => product.Category)
				.FirstOrDefaultAsync(productAttribute => productAttribute.Id == id && productAttribute.IsDeleted == false);

			if (productAttribute == null)
			{
				return new Response<ProductAttributeDto>()
				{
					Status = ResponseStatus.NotFound
				};
			}

			var productAttributeDto = new ProductAttributeDto()
			{
				Id = productAttribute.Id,
				Color = productAttribute.Color,
				Size = productAttribute.Size,
				OriginalPrice = productAttribute.OriginalPrice,
				PriceWithDiscount = productAttribute.PriceWithDiscount,
				ProductName = productAttribute.ProductName,
				ProductCategoryName = productAttribute.ProductCategoryName,
				QuantityLeft = productAttribute.QuantityLeft,
				Image = $"{_productImagesHostname}/{productAttribute.ImageName}",
			};

			return new Response<ProductAttributeDto>()
			{
				Status = ResponseStatus.Success,
				Data = productAttributeDto,
				TotalItemsInDatabase = await _dbContext.Products.Where(product => product.IsDeleted == false).CountAsync(cancellationToken)
			};
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, "Exception: Unable to get all productAttribute");

			return new Response<ProductAttributeDto>()
			{
				Status = ResponseStatus.Exception
			};
		}
	}

	public async Task<ActionResponse> UpdateAsync(UpdateProductAttributeRequest request, CancellationToken cancellationToken)
	{
		try
		{
			var productAttribute = await _dbContext.ProductAttributes
				.FirstOrDefaultAsync(productAttribute => productAttribute.Id == request.Id && productAttribute.IsDeleted == false);

			if (productAttribute == null)
			{
				return new ActionResponse()
				{
					Status = ResponseStatus.NotFound
				};
			}

			// TODO: Implement product attribute update
			throw new NotImplementedException("Update product attribute method is not implemented");

			return new ActionResponse()
			{
				Status = ResponseStatus.Success,
			};
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, "Exception: Unable to update productAttribute");

			return new ActionResponse()
			{
				Status = ResponseStatus.Exception
			};
		}
	}
}
