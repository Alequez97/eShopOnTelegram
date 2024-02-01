using eShopOnTelegram.Domain.Dto.Orders;
using eShopOnTelegram.Domain.Extensions;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.Persistence.Entities.Orders;

namespace eShopOnTelegram.Domain.Services;

public class PaymentService : IPaymentService
{
	private readonly EShopOnTelegramDbContext _dbContext;

	public PaymentService(EShopOnTelegramDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<ActionResponse> UpdateOrderPaymentMethod(string orderNumber, PaymentMethod paymentMethod)
	{
		var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);

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
		await _dbContext.SaveChangesAsync();

		return new ActionResponse
		{
			Status = ResponseStatus.Success
		};
	}

	public async Task<Response<OrderDto>> ConfirmOrderPayment(string orderNumber, PaymentMethod paymentMethod)
	{
		// TODO: UPDLOCK ?
		var order = await _dbContext.Orders
			.Include(order => order.CartItems)
			.ThenInclude(cartItem => cartItem.ProductAttribute)
			.ThenInclude(productAttribute => productAttribute.Product)
			.ThenInclude(product => product.Category)
			.Include(order => order.Customer)
			.FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);

		if (order == null)
		{
			return new Response<OrderDto>
			{
				Status = ResponseStatus.NotFound
			};
		}

		if (order.Status != OrderStatus.AwaitingPayment || order.PaymentMethod != paymentMethod)
		{
			return new Response<OrderDto>()
			{
				Status = ResponseStatus.ValidationFailed
			};
		}

		order.ConfirmPayment();
		await _dbContext.SaveChangesAsync();

		return new Response<OrderDto>
		{
			Status = ResponseStatus.Success,
			Data = order.ToOrderDto(),
		};
	}
}
