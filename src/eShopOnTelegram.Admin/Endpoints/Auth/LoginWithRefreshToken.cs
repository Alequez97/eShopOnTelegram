using eShopOnTelegram.Admin.Constants;
using eShopOnTelegram.Admin.Extensions;
using eShopOnTelegram.Persistence.Context;
using eShopOnTelegram.Utils.Configuration;

using Microsoft.EntityFrameworkCore;

namespace eShopOnTelegram.Admin.Endpoints.Auth;

public class LoginWithRefreshToken : EndpointBaseAsync
	.WithRequest<LoginWithRefreshTokenRequest>
	.WithActionResult<LoginResponse>
{
	private readonly EShopOnTelegramDbContext _eShopOnTelegramDbContext;
	private readonly JWTAuthSettings _authSettings;
	private readonly ILogger<LoginWithRefreshToken> _logger;

	public LoginWithRefreshToken(
		EShopOnTelegramDbContext eShopOnTelegramDbContext,
		AppSettings appSettings,
		ILogger<LoginWithRefreshToken> logger)
	{
		_eShopOnTelegramDbContext = eShopOnTelegramDbContext;
		_authSettings = appSettings.JWTAuthSettings;
		_logger = logger;
	}

	[HttpPost("api/auth/token/refresh")]
	[SwaggerOperation(Tags = new[] { SwaggerGroup.Auth })]
	public override async Task<ActionResult<LoginResponse>> HandleAsync(LoginWithRefreshTokenRequest request, CancellationToken cancellationToken = default)
	{
		try
		{
			var ipAddress = HttpContext.IpAddress();
			ArgumentException.ThrowIfNullOrEmpty(ipAddress);

			var user =
				await _eShopOnTelegramDbContext.Users
					.Include(u => u.UserRefreshTokens)
					.Where(u => u.UserRefreshTokens.Any(rt => rt.Token == request.RefreshToken && rt.RevokedAt == null && DateTime.UtcNow < rt.ExpiresAt))
					.SingleOrDefaultAsync();

			var usedRefreshToken = user?.UserRefreshTokens?.SingleOrDefault(x => x.Token == request.RefreshToken);

			if (user == null || usedRefreshToken == null)
			{
				ModelState.AddModelError("", "Invalid refresh token");
				return ValidationProblem(ModelState);
			}

			var jwt = AuthTokenGenerator.GenerateJToken(user, _authSettings);
			var refreshToken = AuthTokenGenerator.GenerateRefreshToken(ipAddress, _authSettings);

			user.UserRefreshTokens.Add(refreshToken);

			usedRefreshToken.InvalidateRefreshToken(ipAddress, refreshToken.Token);

			await _eShopOnTelegramDbContext.SaveChangesAsync();

			return Ok(new LoginResponse { AccessToken = jwt, RefreshToken = refreshToken.Token });
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to login user with refresh token");
			return new StatusCodeResult(503);
		}
	}
}

public class LoginWithRefreshTokenRequest
{
	public required string RefreshToken { get; set; }
}
