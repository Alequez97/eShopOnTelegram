﻿namespace eShopOnTelegram.Persistence.Entities;

[Index(nameof(TelegramUserUID), IsUnique = true)]
public class Customer : EntityBase
{
	public required long TelegramUserUID { get; set; }

	[MaxLength(100)]
	public string? Username { get; set; }

	[MaxLength(100)]
	public required string FirstName { get; set; }

	[MaxLength(100)]
	public string? LastName { get; set; }
}
