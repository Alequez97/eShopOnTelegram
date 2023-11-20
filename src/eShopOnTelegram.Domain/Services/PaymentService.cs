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

    public async Task UpdateOrderPaymentMethod(string orderNumber, OrderPaymentMethod paymentMethod)
    {
        var order = await _eShopOnTelegramDbContext.Orders.FirstOrDefaultAsync(o => o.OrderNumber == orderNumber) ?? throw new Exception("Order not found.");

        if (order.Status != OrderStatus.New || order.PaymentMethod != OrderPaymentMethod.None)
        {
            throw new Exception("Failed to assign order payment method");
        }

        order.SetPaymentMethod(paymentMethod);
        await _eShopOnTelegramDbContext.SaveChangesAsync();
    }

    public async Task ConfirmOrderPayment(string orderNumber, OrderPaymentMethod paymentMethod)
    {
        // TODO: UPDLOCK ?
        var order = await _eShopOnTelegramDbContext.Orders.FirstOrDefaultAsync(o => o.OrderNumber == orderNumber) ?? throw new Exception("Order not found.");

        if (order.Status != OrderStatus.AwaitingPayment || order.PaymentMethod != paymentMethod)
        {
            throw new Exception("Failed to confirm order payment: status mismatch.");
        }

        order.ConfirmPayment();
        await _eShopOnTelegramDbContext.SaveChangesAsync();
    }
}