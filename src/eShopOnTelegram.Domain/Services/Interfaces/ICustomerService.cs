using eShopOnTelegram.Domain.Dto.Customers;
using eShopOnTelegram.Domain.Requests;
using eShopOnTelegram.Domain.Requests.Customers;

namespace eShopOnTelegram.Domain.Services.Interfaces;

public interface ICustomerService
{
    public Task<Response<IEnumerable<CustomerDto>>> GetMultipleAsync(GetRequest request, CancellationToken cancellationToken);
    public Task<Response> CreateIfNotPresentAsync(CreateCustomerRequest request);
}
