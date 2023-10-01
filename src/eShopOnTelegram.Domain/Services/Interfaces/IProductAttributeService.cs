using eShopOnTelegram.Domain.Dto.ProductAttributes;

namespace eShopOnTelegram.Domain.Services.Interfaces;
public interface IProductAttributeService
{
    Task<Response<ProductAttributeDto>> GetAsync(long id, CancellationToken cancellationToken);
}
