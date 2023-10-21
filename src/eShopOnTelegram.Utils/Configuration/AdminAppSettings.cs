namespace eShopOnTelegram.Utils.Configuration;

public class AdminAppSettings
{
    public required JWTAuthOptions JWTAuthOptions { get; init; }
    public required AdminAzureSettings AzureAppSettings { get; init; }
}

public class JWTAuthOptions
{
    public required string Issuer { get; init; }

    public required string Audience { get; init; }

    public required string Key { get; init; }

    public required int RefreshTokenLifetimeMinutes { get; init; }

    public required int JTokenLifetimeMinutes { get; init; }
}

public class AdminAzureAppSettings
{
    public required string AppInsightsConnectionString { get; init; }

    public required string StorageAccountConnectionString { get; init; }

    public required string RuntimeConfigurationBlobContainerName { get; init; }

    public required string ApplicationContentFileName { get; init; }
}
