using eShopOnTelegram.Admin.Constants;
using eShopOnTelegram.Admin.Extensions;
using eShopOnTelegram.Domain.Dto.ProductsCategories;

using Microsoft.AspNetCore.Authorization;

namespace eShopOnTelegram.Admin.Endpoints.ProductCategories;

public class GetProductCategoryById : EndpointBaseAsync
	.WithRequest<long>
	.WithActionResult<ProductCategoryDto>
{
	private readonly IProductCategoryService _productCategoryService;

	public GetProductCategoryById(IProductCategoryService productCategoryService)
	{
		_productCategoryService = productCategoryService;
	}

	[Authorize]
	[HttpGet("/api/productCategories/{id}")]
	[SwaggerOperation(Tags = new[] { SwaggerGroup.ProductCategories })]
	public override async Task<ActionResult<ProductCategoryDto>> HandleAsync([FromRoute] long id, CancellationToken cancellationToken = default)
	{
		var response = await _productCategoryService.GetAsync(id, cancellationToken);

		return response.AsActionResult();
	}
}
