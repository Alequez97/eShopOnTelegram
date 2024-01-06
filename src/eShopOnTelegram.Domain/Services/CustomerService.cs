using eShopOnTelegram.Domain.Dto.Customers;
using eShopOnTelegram.Domain.Extensions;
using eShopOnTelegram.Domain.Requests;
using eShopOnTelegram.Domain.Requests.Customers;
using eShopOnTelegram.Domain.Services.Interfaces;

namespace eShopOnTelegram.Domain.Services;
public class CustomerService : ICustomerService
{
    private readonly EShopOnTelegramDbContext _dbContext;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(IDbContextFactory<EShopOnTelegramDbContext> dbContextFactory, ILogger<CustomerService> logger)
    {
        _dbContext = dbContextFactory.CreateDbContext();
        _logger = logger;
    }

    public async Task<Response<IEnumerable<CustomerDto>>> GetMultipleAsync(GetRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var products = await _dbContext.Customers
                .WithPagination(request.PaginationModel)
                .ToListAsync(cancellationToken);

            var getCustomersResponse = products.Select(customer => new CustomerDto()
            {
                Id = customer.Id,
                TelegramUserUID = customer.TelegramUserUID,
                Username = customer.Username,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
            });

            return new Response<IEnumerable<CustomerDto>>()
            {
                Status = ResponseStatus.Success,
                Data = getCustomersResponse,
                TotalItemsInDatabase = await _dbContext.Customers.CountAsync(cancellationToken)
            };

        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception: Unable to get all products");

            return new Response<IEnumerable<CustomerDto>>()
            {
                Status = ResponseStatus.Exception
            };
        }
    }

    public async Task<ActionResponse> CreateIfNotPresentAsync(CreateCustomerRequest request)
    {
        try
        {
            // todo maybe it would be better to create a new method that checks whether user exists or not. After, we could reuse it in order service.
            if (await _dbContext.Customers.AnyAsync(c => c.TelegramUserUID == request.TelegramUserUID))
            {
                return new ActionResponse()
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

            return new ActionResponse()
            {
                Status = ResponseStatus.Success,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create new user.");

            return new ActionResponse()
            {
                Status = ResponseStatus.Exception
            };
        }
    }
}
