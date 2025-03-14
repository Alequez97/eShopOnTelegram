﻿using eShopOnTelegram.Admin.Constants;
using eShopOnTelegram.Admin.Extensions;
using eShopOnTelegram.Domain.Dto.Products;
using eShopOnTelegram.Domain.Requests;

using Microsoft.AspNetCore.Authorization;

namespace eShopOnTelegram.Admin.Endpoints.Products;

public class GetProducts : EndpointBaseAsync
	.WithRequest<GetRequest>
	.WithActionResult<IEnumerable<ProductDto>>
{
	private readonly IProductService _productService;

	public GetProducts(IProductService productService)
	{
		_productService = productService;
	}

	[Authorize]
	[HttpGet("/api/products")]
	[SwaggerOperation(Tags = new[] { SwaggerGroup.Products })]
	public async override Task<ActionResult<IEnumerable<ProductDto>>> HandleAsync([FromQuery] GetRequest request, CancellationToken cancellationToken = default)
	{
		var response = await _productService.GetMultipleAsync(request, cancellationToken);

		Response.AddPaginationHeaders(request.PaginationModel, response.TotalItemsInDatabase);

		return response.AsActionResult();
	}
}
