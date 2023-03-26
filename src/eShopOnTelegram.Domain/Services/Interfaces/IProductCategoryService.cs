using eShopOnTelegram.Domain.Requests;
using eShopOnTelegram.Domain.Requests.ProductCategories;
using eShopOnTelegram.Domain.Responses.ProductCategories;

namespace eShopOnTelegram.Domain.Services.Interfaces;

public interface IProductCategoryService
{
    Task<Response<IEnumerable<GetProductCategoryResponse>>> GetMultipleAsync(GetRequest request, CancellationToken cancellationToken);

    Task<Response> CreateAsync(CreateProductCategoryRequest request, CancellationToken cancellationToken);

    Task<Response> DeleteAsync(long id, CancellationToken cancellationToken);
}
