using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Persistence.Entities.Orders;
using eShopOnTelegram.Utils.Configuration;

namespace eShopOnTelegram.Shop.Batch;

public class UnpaidOrdersCleaner : BackgroundService
{
	private readonly IServiceProvider _serviceProvider;
	private readonly ILogger<UnpaidOrdersCleaner> _logger;

	public UnpaidOrdersCleaner(
		IServiceProvider serviceProvider,
		ILogger<UnpaidOrdersCleaner> logger)
	{
		_serviceProvider = serviceProvider;
		_logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		while (!cancellationToken.IsCancellationRequested)
		{

			try
			{
				using IServiceScope scope = _serviceProvider.CreateScope();
				var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
				var appSettings = scope.ServiceProvider.GetRequiredService<AppSettings>();

				var orderConfirmationTimeLimit = TimeSpan.FromMinutes(Convert.ToInt32(appSettings.OrderConfirmationTimeLimitInMinutes));
				var result = await orderService.ProcessUnpaidOrders(orderConfirmationTimeLimit, cancellationToken);

				//var getUnpaidOrdersResponse = await orderService.GetUnpaidOrdersAsync(cancellationToken);

				//if (getUnpaidOrdersResponse.Status != ResponseStatus.Success)
				//{
				//	return;
				//}

				//foreach (var unpaidOrder in getUnpaidOrdersResponse.Data)
				//{
				//	var orderConfirmationTimeLimit = TimeSpan.FromMinutes(Convert.ToInt32(appSettings.OrderConfirmationTimeLimitInMinutes));

				//	var orderCreationTime = unpaidOrder.CreationDate;
				//	var currentTime = DateTime.UtcNow;

				//	var timeElapsed = currentTime - orderCreationTime;

				//	if (timeElapsed > orderConfirmationTimeLimit)
				//	{
				//		await orderService.UpdateStatusAsync(unpaidOrder.OrderNumber, OrderStatus.PaymentIsOverdue, cancellationToken);
				//	}
				//}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "[UnpaidOrdersCleaner]: Failed to clean up unpaid orders");
			}

			await Task.Delay(TimeSpan.FromMinutes(10), cancellationToken);
		}
	}
}
