using eShopOnTelegram.Admin.Extensions;
using eShopOnTelegram.Persistence.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eShopOnTelegram.Admin.Endpoints.Auth;

public class ChangePassword : EndpointBaseAsync
    .WithRequest<ChangePasswordRequest>
    .WithActionResult
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<ChangePassword> _logger;

    public ChangePassword(UserManager<User> userManager, ILogger<ChangePassword> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    [Authorize]
    [HttpPost("api/users/password")]
    public override async Task<ActionResult> HandleAsync(ChangePasswordRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = User.GetUserId();

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            IdentityResult result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                result.Errors.ToList().ForEach(error => ModelState.AddModelError("", error.Description));
                return ValidationProblem(ModelState);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Change password failed.");
            return new StatusCodeResult(503);
        }
    }
}

public class ChangePasswordRequest
{
    public required string CurrentPassword { get; set; }
    public required string NewPassword { get; set; }
}