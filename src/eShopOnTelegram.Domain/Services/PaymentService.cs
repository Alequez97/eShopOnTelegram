using eShopOnTelegram.Domain.Dto.Orders;
using eShopOnTelegram.Domain.Extensions;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.Persistence.Entities.Orders;
using eShopOnTelegram.Persistence.Entities.Payments;
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
			var payment = await _dbContext.Payments
				.Include(payment => payment.Order)
				.FirstOrDefaultAsync(payment => payment.Order.OrderNumber == orderNumber);

			if (payment == null)
			{
				return new ActionResponse
				{
					Status = ResponseStatus.NotFound
				};
			};

			if (payment.Order.Status != OrderStatus.New || payment.PaymentMethod != PaymentMethod.None)
			{
				return new ActionResponse
				{
					Status = ResponseStatus.ValidationFailed,
					ValidationErrors = new()
					{
						$"Payment method already setted up for payment with number {orderNumber}"
					}
				};
			}

			payment.SetPaymentMethod(paymentMethod);
			await _dbContext.SaveChangesAsync();

			return new ActionResponse
			{
				Status = ResponseStatus.Success
			};
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, exception.Message);

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
			var payment = await _dbContext.Payments
				.Include(payment => payment.Order)
				.ThenInclude(order => order.CartItems)
				.ThenInclude(cartItem => cartItem.ProductAttribute)
				.ThenInclude(productAttribute => productAttribute.Product)
				.ThenInclude(product => product.Category)
				.Include(payment => payment.Order)
				.ThenInclude(order => order.Customer)
				.Include(payment => payment.Order)
				.ThenInclude(order => order.PaymentDetails)
				.FirstOrDefaultAsync(payment => payment.Order.OrderNumber == orderNumber);

			if (payment == null)
			{
				return new Response<OrderDto>
				{
					Status = ResponseStatus.NotFound
				};
			}

			if (payment.Order.Status != OrderStatus.AwaitingPayment || payment.PaymentMethod != paymentMethod)
			{
				return new Response<OrderDto>()
				{
					Status = ResponseStatus.ValidationFailed
				};
			}

			payment.ConfirmPayment();
			await _dbContext.SaveChangesAsync();

			return new Response<OrderDto>
			{
				Status = ResponseStatus.Success,
				Data = payment.Order.ToOrderDto(),
			};
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, exception.Message);

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

			var updatedRowsCount = await _dbContext.Payments
				.Include(payment => payment.Order)
				.Where(payment => payment.Order.OrderNumber == orderNumber)
				.ExecuteUpdateAsync(setters => setters.SetProperty(b => b.PaymentValidationToken, encryptedToken));

			if (updatedRowsCount != 1)
			{
				return new ActionResponse()
				{
					Status = ResponseStatus.Exception
				};
			}

			return new ActionResponse()
			{
				Status = ResponseStatus.Success
			};
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, exception.Message);

			return new ActionResponse()
			{
				Status = ResponseStatus.Exception
			};
		}
	}

	public async Task<ActionResponse> UpdateInvoiceUrlAsync(string orderNumber, string invoiceUrl, CancellationToken cancellationToken)
	{
		try
		{
			var updatedRowsCount = await _dbContext.Payments
				.Include(payment => payment.Order)
				.Where(payment => payment.Order.OrderNumber == orderNumber)
				.ExecuteUpdateAsync(setters => setters.SetProperty(b => b.InvoiceUrl, invoiceUrl));

			if (updatedRowsCount != 1)
			{
				return new ActionResponse()
				{
					Status = ResponseStatus.Exception
				};
			}

			return new ActionResponse()
			{
				Status = ResponseStatus.Success
			};
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, exception.Message);

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
			var payment = await _dbContext.Payments
				.Include(payment => payment.Order)
				.FirstOrDefaultAsync(payment => payment.Order.OrderNumber == orderNumber, cancellationToken);

			if (payment == null)
			{
				return new Response<string>
				{
					Status = ResponseStatus.NotFound
				};
			};

			if (payment.PaymentValidationToken == null)
			{
				return new Response<string>
				{
					Status = ResponseStatus.NotFound
				};
			}
			
			return new Response<string>()
			{
				Status = ResponseStatus.Success,
				Data = _symmetricEncryptionService.Decrypt(payment.PaymentValidationToken),
			};
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, exception.Message);

			return new Response<string>()
			{
				Status = ResponseStatus.Exception
			};
		}
	}
}
