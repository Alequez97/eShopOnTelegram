using Microsoft.AspNetCore.Identity;

namespace eShopOnTelegram.Persistence.Entities.Users;
public class User : IdentityUser<long>
{
	public ICollection<IdentityUserClaim<long>> Claims { get; set; } = new List<IdentityUserClaim<long>>();
	public ICollection<UserRefreshToken> UserRefreshTokens { get; set; } = new List<UserRefreshToken>();
}
