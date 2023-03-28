using eShopOnTelegram.Domain.Requests;
using eShopOnTelegram.Domain.Requests.Customers;
using eShopOnTelegram.Domain.Responses.Customers;

namespace eShopOnTelegram.Domain.Services.Interfaces;

public interface ICustomerService
{
    public Task<Response<IEnumerable<GetCustomersResponse>>> GetMultipleAsync(GetRequest request, CancellationToken cancellationToken);
    public Task<Response> CreateUserIfNotPresentAsync(CreateCustomerRequest request);
}
