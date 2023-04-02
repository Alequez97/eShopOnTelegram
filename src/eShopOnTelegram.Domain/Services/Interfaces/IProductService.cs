using eShopOnTelegram.Domain.Dto.Products;
using eShopOnTelegram.Domain.Requests;
using eShopOnTelegram.Domain.Requests.Products;

namespace eShopOnTelegram.Domain.Services.Interfaces;

public interface IProductService
{
    Task<Response<ProductDto>> GetByIdAsync(long id, CancellationToken cancellationToken);

    Task<Response<IEnumerable<ProductDto>>> GetMultipleAsync(GetRequest request, CancellationToken cancellationToken);

    Task<Response<IEnumerable<ProductDto>>> GetAllAsync(CancellationToken cancellationToken);

    Task<ActionResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken);

    Task<ActionResponse> UpdateAsync(UpdateProductRequest request, CancellationToken cancellationToken);

    Task<ActionResponse> DeleteAsync(long id, CancellationToken cancellationToken);
}