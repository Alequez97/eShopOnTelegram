using eShopOnTelegram.Admin.Constants;
using eShopOnTelegram.Persistence.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace eShopOnTelegram.Admin.Endpoints.Auth;

public class CreateUser : EndpointBaseAsync
    .WithRequest<CreateUserRequest>
    .WithActionResult
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<CreateUser> _logger;

    public CreateUser(UserManager<User> userManager, ILogger<CreateUser> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    [Authorize(Policy = AuthPolicy.RequireSuperadminClaim)]
    [HttpPost("api/users")]
    public override async Task<ActionResult> HandleAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var newAdmin = new User 
            { 
                UserName = request.UserName,
                Claims = new List<IdentityUserClaim<long>> { new IdentityUserClaim<long> { ClaimType = "Role", ClaimValue = "admin" } }
            };

            IdentityResult result = await _userManager.CreateAsync(newAdmin, request.Password);
            if (!result.Succeeded)
            {
                result.Errors.ToList().ForEach(error => ModelState.AddModelError("", error.Description));
                return ValidationProblem(ModelState);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create user");
            return new StatusCodeResult(503);
        }
    }
}

public class CreateUserRequest
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
}