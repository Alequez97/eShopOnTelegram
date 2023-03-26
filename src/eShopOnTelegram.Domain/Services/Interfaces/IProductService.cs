using eShopOnTelegram.Domain.Requests;
using eShopOnTelegram.Domain.Requests.Products;
using eShopOnTelegram.Domain.Responses.Products;

namespace eShopOnTelegram.Domain.Services.Interfaces;

public interface IProductService
{
    Task<Response<GetProductResponse>> GetByIdAsync(long id, CancellationToken cancellationToken);

    Task<Response<IEnumerable<GetProductResponse>>> GetMultipleAsync(GetRequest request, CancellationToken cancellationToken);

    Task<CreateResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken);

    Task<Response> UpdateAsync(UpdateProductRequest request, CancellationToken cancellationToken);

    Task<Response> DeleteAsync(long id, CancellationToken cancellationToken);
}