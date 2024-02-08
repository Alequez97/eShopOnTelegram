using eShopOnTelegram.Domain.Dto.Orders;
using eShopOnTelegram.Persistence.Entities.Payments;

namespace eShopOnTelegram.Domain.Services.Interfaces;

public interface IPaymentService
{
	Task<ActionResponse> UpdateOrderPaymentMethodAsync(string orderNumber, PaymentMethod paymentMethod, CancellationToken cancellationToken);
	Task<Response<OrderDto>> ConfirmOrderPaymentAsync(string orderNumber, PaymentMethod paymentMethod, CancellationToken cancellationToken);
	Task<ActionResponse> UpdateValidationTokenAsync(string orderNumber, string validationToken, CancellationToken cancellationToken);
	Task<ActionResponse> UpdateInvoiceUrlAsync(string orderNumber, string invoiceUrl, CancellationToken cancellationToken);
	Task<Response<string>> GetValidationTokenAsync(string orderNumber, CancellationToken cancellationToken);
}
