using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.Persistence.Entities.Orders;

namespace eShopOnTelegram.Domain.Services;

public class PaymentService : IPaymentService
{
    private readonly EShopOnTelegramDbContext _eShopOnTelegramDbContext;

    public PaymentService(EShopOnTelegramDbContext eShopOnTelegramDbContext)
    {
        _eShopOnTelegramDbContext = eShopOnTelegramDbContext;
    }

    public async Task<ActionResponse> UpdateOrderPaymentMethod(string orderNumber, PaymentMethod paymentMethod)
    {
        var order = await _eShopOnTelegramDbContext.Orders.FirstOrDefaultAsync(o => o.OrderNumber == orderNumber); /* ?? throw new Exception("Order not found.");*/

        if (order == null)
        {
            return new ActionResponse
            {
                Status = ResponseStatus.NotFound
            };
        };

        if (order.Status != OrderStatus.New || order.PaymentMethod != PaymentMethod.None)
        {
            return new ActionResponse
            {
                Status = ResponseStatus.ValidationFailed
            };
        }

        order.SetPaymentMethod(paymentMethod);
        await _eShopOnTelegramDbContext.SaveChangesAsync();

        return new ActionResponse
        {
            Status = ResponseStatus.Success
        };
    }

    public async Task<ActionResponse> ConfirmOrderPayment(string orderNumber, PaymentMethod paymentMethod)
    {
        // TODO: UPDLOCK ?
        var order = await _eShopOnTelegramDbContext.Orders.FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
        if (order == null)
        {
            return new ActionResponse
            {
                Status = ResponseStatus.NotFound
            };
        }

        if (order.Status != OrderStatus.AwaitingPayment || order.PaymentMethod != paymentMethod)
        {
            return new ActionResponse()
            {
                Status = ResponseStatus.ValidationFailed
            };
        }

        order.ConfirmPayment();
        await _eShopOnTelegramDbContext.SaveChangesAsync();

        return new ActionResponse
        {
            Status = ResponseStatus.Success
        };
    }
}
