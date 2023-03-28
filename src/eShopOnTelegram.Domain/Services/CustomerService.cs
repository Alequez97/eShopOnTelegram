using eShopOnTelegram.Domain.Extensions;
using eShopOnTelegram.Domain.Requests;
using eShopOnTelegram.Domain.Requests.Customers;
using eShopOnTelegram.Domain.Responses.Customers;
using eShopOnTelegram.Domain.Services.Interfaces;

namespace eShopOnTelegram.Domain.Services;
public class CustomerService : ICustomerService
{
    private readonly EShopOnTelegramDbContext _dbContext;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(EShopOnTelegramDbContext dbContext, ILogger<CustomerService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Response<IEnumerable<GetCustomersResponse>>> GetMultipleAsync(GetRequest request, CancellationToken cancellationToken)
    {
        var response = new Response<IEnumerable<GetCustomersResponse>>();

        try
        {
            var products = await _dbContext.Customers
                .WithPagination(request.PaginationModel)
                .ToListAsync(cancellationToken);

            var getCustomersResponse = products.Select(customer => new GetCustomersResponse
            {
                TelegramUserUID = customer.TelegramUserUID,
                Username = customer.Username,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
            });

            response.Status = ResponseStatus.Success;
            response.Data = getCustomersResponse;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception: Unable to get all products");
            response.Status = ResponseStatus.Exception;
        }

        return response;
    }

    public async Task<Response> CreateUserIfNotPresentAsync(CreateCustomerRequest request)
    {
        try
        {
            // todo maybe it would be better to create a new method that checks whether user exists or not. After, we could reuse it in order service.
            if (await _dbContext.Customers.AnyAsync(c => c.TelegramUserUID == request.TelegramUserUID))
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
            _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync();

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