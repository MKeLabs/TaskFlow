using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TaskFlow.API.Auth;
using TaskFlow.API.Options;
using TaskFlow.API.Services;
using TaskFlow.DAL.Auth;
using TaskFlow.DAL.Entities;

namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuthController(
    UserManager<ApplicationUser> userManager,
    IJwtTokenService jwtTokenService,
    IOptions<JwtOptions> jwtOptions) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<LoginResponse>> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser { UserName = request.Email, Email = request.Email };
        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return BadRequest(new { errors = result.Errors.Select(e => e.Description).ToList() });
        }

        await userManager.AddToRoleAsync(user, AppRoles.User);

        var roles = await userManager.GetRolesAsync(user);
        var token = jwtTokenService.CreateAccessToken(user.Id, user.Email ?? request.Email, roles);
        var expires = DateTimeOffset.UtcNow.AddMinutes(jwtOptions.Value.AccessTokenMinutes);

        return Ok(new LoginResponse(token, expires, user.Email ?? request.Email, roles.ToList()));
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Unauthorized();
        }

        var valid = await userManager.CheckPasswordAsync(user, request.Password);
        if (!valid)
        {
            return Unauthorized();
        }

        var roles = await userManager.GetRolesAsync(user);
        var token = jwtTokenService.CreateAccessToken(user.Id, user.Email ?? request.Email, roles);
        var expires = DateTimeOffset.UtcNow.AddMinutes(jwtOptions.Value.AccessTokenMinutes);

        return Ok(new LoginResponse(token, expires, user.Email ?? request.Email, roles.ToList()));
    }

    [HttpGet("me")]
    public IActionResult Me()
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var email = User.FindFirstValue(ClaimTypes.Email);
        var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
        return Ok(new { id, email, roles });
    }
}
