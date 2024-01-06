using eShopOnTelegram.Domain.Dto.ProductAttributes;
using eShopOnTelegram.Domain.Requests.ProductAttributes;

namespace eShopOnTelegram.Domain.Services.Interfaces;

public interface IProductAttributeService
{
    Task<Response<ProductAttributeDto>> GetAsync(long id, CancellationToken cancellationToken);

    Task<ActionResponse> UpdateAsync(UpdateProductAttributeRequest request, CancellationToken cancellationToken);
}
