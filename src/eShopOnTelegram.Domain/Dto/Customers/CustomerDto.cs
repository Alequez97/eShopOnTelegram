namespace eShopOnTelegram.Domain.Dto.Customers;

public class CustomerDto : DtoBase
{
	public required long TelegramUserUID { get; set; }

	public string? Username { get; set; }

	public required string FirstName { get; set; }

	public string? LastName { get; set; }
}
