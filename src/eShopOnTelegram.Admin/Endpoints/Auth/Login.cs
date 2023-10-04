﻿using eShopOnTelegram.Admin.Constants;
using eShopOnTelegram.Admin.Extensions;
using eShopOnTelegram.Persistence.Entities;

using Microsoft.AspNetCore.Identity;

namespace eShopOnTelegram.Admin.Endpoints.Auth;

public class Login : EndpointBaseAsync
    .WithRequest<LoginRequest>
    .WithActionResult<LoginResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly JWTAuthOptions _authOptions;
    private readonly ILogger<LoginRequest> _logger;

    public Login(
        UserManager<User> userManager,
        AppSettings appSettings,
        ILogger<LoginRequest> logger)
    {
        _userManager = userManager;
        _authOptions = appSettings.JWTAuthOptions;
        _logger = logger;
    }

    [HttpPost("api/auth/login")]
    [SwaggerOperation(Tags = new[] { SwaggerGroup.Auth })]
    public override async Task<ActionResult<LoginResponse>> HandleAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var ipAddress = HttpContext.IpAddress();
            ArgumentException.ThrowIfNullOrEmpty(ipAddress);

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Incorrect credentials");
                return ValidationProblem(ModelState);
            }

            if (!await _userManager.CheckPasswordAsync(user, request.Password))
            {
                ModelState.AddModelError("", "Incorrect credentials");
                return ValidationProblem(ModelState);
            }

            var jwtToken = AuthTokenGenerator.GenerateJToken(user, _authOptions);
            var refreshToken = AuthTokenGenerator.GenerateRefreshToken(ipAddress, _authOptions);

            user.UserRefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            var response = new LoginResponse
            {
                AccessToken = jwtToken,
                RefreshToken = refreshToken.Token
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login failed");
            return new StatusCodeResult(503);
        }
    }
}

public class LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class LoginResponse
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}