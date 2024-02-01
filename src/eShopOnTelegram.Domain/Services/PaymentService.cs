using eShopOnTelegram.Domain.Dto.Orders;
using eShopOnTelegram.Domain.Extensions;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.Persistence.Entities.Orders;
using eShopOnTelegram.Utils.Encryption.Interfaces;

namespace eShopOnTelegram.Domain.Services;

public class PaymentService : IPaymentService
{
	private readonly EShopOnTelegramDbContext _dbContext;
	private readonly ISymmetricEncryptionService _symmetricEncryptionService;
	private readonly ILogger<PaymentService> _logger;

	public PaymentService(
		EShopOnTelegramDbContext dbContext,
		ISymmetricEncryptionService symmetricEncryptionService,
		ILogger<PaymentService> logger)
	{
		_dbContext = dbContext;
		_symmetricEncryptionService = symmetricEncryptionService;
		_logger = logger;
	}

	public async Task<ActionResponse> UpdateOrderPaymentMethodAsync(string orderNumber, PaymentMethod paymentMethod, CancellationToken cancellationToken)
	{
		try
		{
			var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.OrderNumber == orderNumber, cancellationToken);

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
		catch (Exception exception)
		{
			_logger.LogError(exception, "Exception: Unable to update order payment method");

			return new ActionResponse()
			{
				Status = ResponseStatus.Exception
			};
		}
	}

	public async Task<Response<OrderDto>> ConfirmOrderPaymentAsync(string orderNumber, PaymentMethod paymentMethod, CancellationToken cancellationToken)
	{
		try
		{
			// TODO: UPDLOCK ?
			var order = await _dbContext.Orders
				.Include(order => order.CartItems)
				.ThenInclude(cartItem => cartItem.ProductAttribute)
				.ThenInclude(productAttribute => productAttribute.Product)
				.ThenInclude(product => product.Category)
				.Include(order => order.Customer)
				.FirstOrDefaultAsync(o => o.OrderNumber == orderNumber, cancellationToken);

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
		catch (Exception exception)
		{
			_logger.LogError(exception, "Exception: Unable to confirm order payment");

			return new Response<OrderDto>
			{
				Status = ResponseStatus.Exception
			};
		}
	}

	public async Task<ActionResponse> UpdateValidationTokenAsync(string orderNumber, string validationToken, CancellationToken cancellationToken)
	{
		try
		{
			var encryptedToken = _symmetricEncryptionService.Encrypt(validationToken);

			await _dbContext.Orders
				.Where(order => order.OrderNumber == orderNumber)
				.ExecuteUpdateAsync(setters => setters.SetProperty(b => b.PaymentValidationToken, encryptedToken));

			return new ActionResponse()
			{
				Status = ResponseStatus.Success
			};
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, "Exception: Unable to update order payment method");

			return new ActionResponse()
			{
				Status = ResponseStatus.Exception
			};
		}
	}

	public async Task<Response<string>> GetValidationTokenAsync(string orderNumber, CancellationToken cancellationToken)
	{
		try
		{
			var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.OrderNumber == orderNumber, cancellationToken);

			if (order == null)
			{
				return new Response<string>
				{
					Status = ResponseStatus.NotFound
				};
			};

			if (order.PaymentValidationToken == null)
			{
				return new Response<string>
				{
					Status = ResponseStatus.NotFound
				};
			}
			
			return new Response<string>()
			{
				Status = ResponseStatus.Success,
				Data = _symmetricEncryptionService.Decrypt(order.PaymentValidationToken),
			};
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, "Exception: Unable to update order payment method");

			return new Response<string>()
			{
				Status = ResponseStatus.Exception
			};
		}
	}
}
