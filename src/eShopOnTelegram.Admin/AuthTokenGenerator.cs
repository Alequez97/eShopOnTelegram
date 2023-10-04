using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using eShopOnTelegram.Persistence.Entities;

using Microsoft.IdentityModel.Tokens;

namespace eShopOnTelegram.Admin;

public static class AuthTokenGenerator
{
    public static string GenerateJToken(User user, JWTAuthOptions authOptions)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(authOptions.Key);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, user.Email!), // Email can be null
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(authOptions.JTokenLifetimeMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = authOptions.Issuer,
            Audience = authOptions.Audience
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        return jwt;
    }

    public static UserRefreshToken GenerateRefreshToken(string ipAddress, JWTAuthOptions authOptions)
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