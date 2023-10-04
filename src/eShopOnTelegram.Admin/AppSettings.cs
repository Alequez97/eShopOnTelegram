namespace eShopOnTelegram.Admin;

public class AppSettings
{
    public required JWTAuthOptions JWTAuthOptions { get; set; }
}

public class JWTAuthOptions
{
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required string Key { get; init; }
    public required int RefreshTokenLifetimeMinutes { get; init; }
    public required int JTokenLifetimeMinutes { get; init; }
}