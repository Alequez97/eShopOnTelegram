using Microsoft.AspNetCore.Identity;

namespace eShopOnTelegram.Persistence.Entities;
public class User : IdentityUser<long>
{
    public ICollection<UserRefreshToken> UserRefreshTokens { get; set; } = new List<UserRefreshToken>();
}