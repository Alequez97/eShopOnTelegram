namespace eShopOnTelegram.Domain.Responses.Customers;

public class GetCustomersResponse : Response
{
    public required long TelegramUserUID { get; set; }

    public string? Username { get; set; }

    public required string FirstName { get; set; }

    public string? LastName { get; set; }
}
