using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs;
using WMS.Application.Interfaces;

namespace WMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            var result = await _authService.LoginAsync(request);
            if (result == null)
                return Unauthorized(new { message = "Invalid username or password." });
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login failed for user {Username}", request.Username);
            return StatusCode(500, new { message = "An error occurred during login." });
        }
    }
}
