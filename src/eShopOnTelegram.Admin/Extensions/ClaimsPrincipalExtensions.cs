using System.Security.Claims;

namespace eShopOnTelegram.Admin.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static long GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        ArgumentNullException.ThrowIfNull(claimsPrincipal, nameof(claimsPrincipal));

        var nameIdentifierValue = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ArgumentException.ThrowIfNullOrEmpty(nameIdentifierValue, nameof(nameIdentifierValue));

        long userId = long.Parse(nameIdentifierValue);
        return userId;
    }
}