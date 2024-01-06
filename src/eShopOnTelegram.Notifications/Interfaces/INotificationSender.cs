namespace eShopOnTelegram.Notifications.Interfaces;

public interface INotificationSender
{
	Task SendNotificationAsync(string title, string message, CancellationToken cancellationToken);

	Task SendOrderReceivedNotificationAsync(string orderNumber, CancellationToken cancellationToken);
}
