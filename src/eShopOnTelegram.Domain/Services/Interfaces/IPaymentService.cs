using eShopOnTelegram.Persistence.Entities.Orders;

namespace eShopOnTelegram.Domain.Services.Interfaces;

public interface IPaymentService
{
	Task<ActionResponse> UpdateOrderPaymentMethod(string orderNumber, PaymentMethod paymentMethod);
	Task<ActionResponse> ConfirmOrderPayment(string orderNumber, PaymentMethod paymentMethod);
}
