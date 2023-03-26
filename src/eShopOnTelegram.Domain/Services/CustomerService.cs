using eShopOnTelegram.Domain.Requests.Customers;
using eShopOnTelegram.Domain.Services.Interfaces;

namespace eShopOnTelegram.Domain.Services;
public class CustomerService : ICustomerService
{
    private readonly EShopOnTelegramDbContext _ctx;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(EShopOnTelegramDbContext ctx, ILogger<CustomerService> logger)
    {
        _ctx = ctx;
        _logger = logger;
    }

    public async Task<Response> CreateUserIfNotPresent(CreateCustomerRequest request)
    {
        try
        {
            // todo maybe it would be better to create a new method that checks whether user exists or not. After, we could reuse it in order service.
            if (!await _ctx.Customers.AnyAsync(c => c.TelegramUserUID == request.TelegramUserUID))
            {
                return new Response()
                {
                    Status = ResponseStatus.Success,
                };
            }

            var customer = new Customer()
            {
                TelegramUserUID = request.TelegramUserUID,
                LastName = request.LastName,
                FirstName = request.FirstName,
                Username = request.Username
            };
            _ctx.Customers.Add(customer);
            await _ctx.SaveChangesAsync();

            return new Response()
            {
                Status = ResponseStatus.Success,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create new user.");
            return new Response() { Status = ResponseStatus.Exception };
        }
    }
}