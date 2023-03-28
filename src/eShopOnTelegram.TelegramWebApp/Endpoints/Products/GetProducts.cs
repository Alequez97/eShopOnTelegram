﻿using eShopOnTelegram.Domain.Requests;
using eShopOnTelegram.TelegramWebApp.Constants;
using eShopOnTelegram.TelegramWebApp.Extensions;

namespace eShopOnTelegram.TelegramWebApp.Endpoints.Products;

public class GetProducts : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<IEnumerable<GetProductResponse>>
{
    private readonly IProductService _productService;

    public GetProducts(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("/api/products")]
    [SwaggerOperation(Tags = new[] { SwaggerGroup.Products })]
    public async override Task<ActionResult<IEnumerable<GetProductResponse>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var response = await _productService.GetAllAsync(cancellationToken);
        return response.AsActionResult();
    }
}