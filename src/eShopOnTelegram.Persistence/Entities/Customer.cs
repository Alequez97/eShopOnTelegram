namespace eShopOnTelegram.Persistence.Entities;

[Index(nameof(TelegramUserUID), IsUnique = true)]
public class Customer : EntityBase
{
    public Customer(string? username, string firstName, string? lastName, long telegramUserUID)
    {
        Username = username;
        FirstName = firstName;
        LastName = lastName;
        TelegramUserUID = telegramUserUID;
    }

    public string? Username { get; set; }

    [MaxLength(100)]
    public string FirstName { get; set; }

    [MaxLength(100)]
    public string? LastName { get; set; }

    public long TelegramUserUID { get; set; }
}
