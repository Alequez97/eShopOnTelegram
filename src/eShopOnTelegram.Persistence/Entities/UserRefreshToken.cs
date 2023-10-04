namespace eShopOnTelegram.Persistence.Entities;

[Index(nameof(Token))]
public class UserRefreshToken : EntityBase
{
    public long UserId { get; init; }
    public User User { get; set; }

    public required string Token { get; init; }
    public required string CreatedByIP { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required DateTimeOffset ExpiresAt { get; init; }

    public DateTimeOffset? RevokedAt { get; private set; }
    public string? RevokedByIp { get; private set; }
    public string? ReplacedByToken { get; private set; }

    public void InvalidateRefreshToken(string ipAddress, string replacedByToken)
    {
        RevokedAt = DateTimeOffset.UtcNow;
        RevokedByIp = ipAddress;
        ReplacedByToken = replacedByToken;
    }
}