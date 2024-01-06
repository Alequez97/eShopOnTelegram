using eShopOnTelegram.Domain.Dto.ProductsCategories;
using eShopOnTelegram.Domain.Requests;
using eShopOnTelegram.Domain.Requests.ProductCategories;

namespace eShopOnTelegram.Domain.Services.Interfaces;

public interface IProductCategoryService
{
	Task<Response<ProductCategoryDto>> GetAsync(long id, CancellationToken cancellationToken);

	Task<Response<IEnumerable<ProductCategoryDto>>> GetMultipleAsync(GetRequest request, CancellationToken cancellationToken);

	Task<ActionResponse> CreateAsync(CreateProductCategoryRequest request, CancellationToken cancellationToken);

	Task<ActionResponse> UpdateAsync(UpdateProductCategoryRequest request, CancellationToken cancellationToken);

	Task<ActionResponse> DeleteAsync(long id, CancellationToken cancellationToken);
}
