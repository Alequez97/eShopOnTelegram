using eShopOnTelegram.Domain.Dto.ProductAttributes;
using eShopOnTelegram.Domain.Dto.Products;
using eShopOnTelegram.Persistence.Entities.Products;

namespace eShopOnTelegram.Domain.Extensions;
public static class ProductExtensions
{
    public static ProductDto ToProductDto(this Product product, string productImagesHostname)
    {
        return new ProductDto()
        {
            Id = product.Id,
            Name = product.Name,
            ProductCategoryName = product.Category.Name,
            ProductAttributes = product.ProductAttributes.Select(productAttribute => new ProductAttributeDto()
            {
                Id = productAttribute.Id,
                ProductName = productAttribute.ProductName,
                ProductCategoryName = productAttribute.ProductCategoryName,
                Color = productAttribute.Color,
                Size = productAttribute.Size,
                OriginalPrice = productAttribute.OriginalPrice,
                PriceWithDiscount = productAttribute.PriceWithDiscount,
                QuantityLeft = productAttribute.QuantityLeft,
                Image = $"{productImagesHostname}/{productAttribute.ImageName}",
            })
        };
    }
}
