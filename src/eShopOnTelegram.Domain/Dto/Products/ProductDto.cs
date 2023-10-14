using eShopOnTelegram.Domain.Dto.ProductAttributes;

namespace eShopOnTelegram.Domain.Dto.Products; 

public class ProductDto : DtoBase
{
    public required string Name { get; set; }

    public required string ProductCategoryName { get; set; }

    public required IEnumerable<ProductAttributeDto> ProductAttributes { get; set; }
}
