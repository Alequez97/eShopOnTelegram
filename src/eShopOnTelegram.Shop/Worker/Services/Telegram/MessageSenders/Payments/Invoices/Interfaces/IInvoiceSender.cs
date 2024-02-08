using eShopOnTelegram.Domain.Dto.Orders;
using eShopOnTelegram.Persistence.Entities.Payments;

namespace eShopOnTelegram.Shop.Worker.Services.Telegram.MessageSenders.Payments.Invoices.Interfaces;

public interface IInvoiceSender
{
	public Task SendInvoiceAsync(long telegramId, OrderDto orderDto, CancellationToken cancellationToken);

	public PaymentMethod PaymentMethod { get; }
}
