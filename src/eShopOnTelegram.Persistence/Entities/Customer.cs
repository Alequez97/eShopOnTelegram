namespace eShopOnTelegram.Persistence.Entities;

public class Customer : EntityBase
{
    [MaxLength(100)]
    public string? Username { get; set; }

    [MaxLength(100)]
    public string FirstName { get; set; }

    [MaxLength(100)]
    public string LastName { get; set; }

    [MaxLength(100)]
    public string TelegramId { get; set; }
}
