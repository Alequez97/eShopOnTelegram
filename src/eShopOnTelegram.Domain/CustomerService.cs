using eShopOnTelegram.Persistence.Context;
using eShopOnTelegram.Persistence.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace eShopOnTelegram.Domain;
internal class CustomerService
{
    private readonly EShopOnTelegramDbContext _ctx;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(EShopOnTelegramDbContext ctx, ILogger<CustomerService> logger)
    {
        _ctx = ctx;
        _logger = logger;
    }

    // here we need to define some response i suppose
    public async Task CreateUserIfNotPresent(User telegramUser)
    {
        if (!await _ctx.Customers.AnyAsync(c => c.TelegramUserUID == senderUID))
        {
            try
            {

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create new user.");
            }
            var customer = new Customer();
            _ctx.Customers.Add(customer);
            await _ctx.SaveChangesAsync();
        }
        else
        {

        }
    }
}
