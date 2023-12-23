using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using eShopOnTelegram.Persistence.Entities.Users;
using eShopOnTelegram.Utils.Configuration;

using Microsoft.IdentityModel.Tokens;

namespace eShopOnTelegram.Admin;

public static class AuthTokenGenerator
{
    public static string GenerateJToken(User user, JWTAuthSettings authSettings)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(authSettings.Key);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, user.UserName), // Email can be null
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        if(user.Claims.Any())
        {
            var roleClaim = user.Claims.Where(c => c.ClaimType == "Role").SingleOrDefault();
            if(roleClaim is not null)
            {
                ArgumentException.ThrowIfNullOrEmpty(roleClaim.ClaimValue);
                claims.Add(new Claim(ClaimTypes.Role, roleClaim.ClaimValue));
            }
        }

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(authSettings.JTokenLifetimeMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = authSettings.Issuer,
            Audience = authSettings.Audience
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        return jwt;
    }

    public static UserRefreshToken GenerateRefreshToken(string ipAddress, JWTAuthSettings authOptions)
    {
        var randomBytes = new byte[64];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        return new UserRefreshToken
        {
            Token = Convert.ToBase64String(randomBytes),
            ExpiresAt = DateTime.UtcNow.AddMinutes(authOptions.RefreshTokenLifetimeMinutes),
            CreatedAt = DateTime.UtcNow,
            CreatedByIP = ipAddress
        };
    }
}