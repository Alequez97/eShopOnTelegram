using eShopOnTelegram.Domain.Requests.Customers;

namespace eShopOnTelegram.Domain.Services.Interfaces;

public interface ICustomerService
{
    public Task<Response> CreateUserIfNotPresent(CreateCustomerRequest request);
}
