using eShopOnTelegram.Persistence.Entities.Orders;

namespace eShopOnTelegram.Domain.Services.Interfaces;

public interface IPaymentService
{
    Task UpdateOrderPaymentMethod(string orderNumber, OrderPaymentMethod paymentMethod);
    Task ConfirmOrderPayment(string orderNumber, OrderPaymentMethod paymentMethod);
}