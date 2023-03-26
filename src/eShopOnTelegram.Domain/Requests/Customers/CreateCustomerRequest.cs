namespace eShopOnTelegram.Domain.Requests.Customers;

public class CreateCustomerRequest
{
    public long TelegramUserUID { get; set; }
    public string Username { get; set; }
    public string FirstName{ get; set; }
    public string LastName { get; set; }
}
