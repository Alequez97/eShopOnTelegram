using eShopOnTelegram.Domain.Dto.Orders;
using eShopOnTelegram.Persistence.Entities.Payments;
using eShopOnTelegram.Shop.Worker.Services.Telegram.MessageSenders.Payments.Invoices.Interfaces;

namespace eShopOnTelegram.Shop.Worker.Services.Telegram.MessageSenders.Payments.Invoices;

public class CoinGateInvoiceSender : IInvoiceSender
{
	public Task SendInvoiceAsync(long telegramId, OrderDto orderDto, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}

	public PaymentMethod PaymentMethod => PaymentMethod.CoinGate;

}
