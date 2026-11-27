using Keeply.Application.DTOs.Auth;
using Keeply.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Keeply.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);
        return result is null ? Problem(title: "Email already exists.", statusCode: StatusCodes.Status409Conflict) : Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);
        return result is null ? Unauthorized() : Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
    {
        var result = await _authService.RefreshAsync(request);
        return result is null ? Unauthorized() : Ok(result);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] RefreshRequest request)
    {
        var success = await _authService.LogoutAsync(request.RefreshToken);
        return success ? NoContent() : Unauthorized();
    }
}
