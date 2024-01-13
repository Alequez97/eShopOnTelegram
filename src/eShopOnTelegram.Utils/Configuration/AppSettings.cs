namespace eShopOnTelegram.Utils.Configuration;

public class AppSettings
{
	public required AzureSettings AzureSettings { get; init; }
	public required JWTAuthSettings JWTAuthSettings { get; init; }
	public required PaymentSettings PaymentSettings { get; init; }
	public required TelegramBotSettings TelegramBotSettings { get; init; }
	public required string ProductImagesHostName { get; set; }
	public required string AdminAppHostName { get; set; }
	public required string Language { get; set; }
}

public class AzureSettings
{
	public required string AppInsightsConnectionString { get; init; }

	public required string StorageAccountConnectionString { get; init; }

	public required string RuntimeConfigurationBlobContainerName { get; init; }

	public required string ProductImagesBlobContainerName { get; init; }

	public required string KeyVaultUri { get; init; }

	public required string TenantId { get; init; }

	public required string ClientId { get; init; }

	public required string ClientSecret { get; init; }

	public required string ResourceGroupName { get; init; }

	public required string ShopAppServiceName { get; init; }
}

public class JWTAuthSettings
{
	public required string Issuer { get; init; }

	public required string Audience { get; init; }

	public required string Key { get; init; }

	public required int RefreshTokenLifetimeMinutes { get; init; }

	public required int JTokenLifetimeMinutes { get; init; }
}

public class PaymentSettings
{
	public required string MainCurrency { get; init; }

	public required Card Card { get; init; }

	public required Plisio Plisio { get; init; }

	public required bool PaymentThroughSellerEnabled { get; init; }

	public bool AllPaymentsDisabled => !Card.Enabled && !Plisio.Enabled && !PaymentThroughSellerEnabled;
}

public class Card
{
	public bool Enabled => !string.IsNullOrWhiteSpace(ApiToken);

	public required string ApiToken { get; init; }
}

public class Plisio
{
	public bool Enabled => !string.IsNullOrWhiteSpace(ApiToken);

	public required string ApiToken { get; init; }

	public required string CryptoCurrency { get; init; }
}

public class TelegramBotSettings
{
	public required string BotOwnerTelegramId { get; init; }

	public required string Token { get; init; }

	public required string WebAppUrl { get; init; }
}
