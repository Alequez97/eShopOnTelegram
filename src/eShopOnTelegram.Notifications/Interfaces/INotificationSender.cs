namespace eShopOnTelegram.Notifications.Interfaces;

public interface INotificationSender
{
    void SendNotificationAsync(string title, string message);
}
