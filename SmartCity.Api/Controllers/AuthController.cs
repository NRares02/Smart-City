using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCity.Api.DTOs.Auth;
using SmartCity.Api.Repositories;
using SmartCity.Api.Services;
using System.Security.Claims;

namespace SmartCity.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    // TODO: resolve the citizen role dynamically from the roles collection instead of hardcoding it
    private const string CitizenRoleId = "6a8c294abb8d69e69c4d93ef";

    private readonly AuthService _authService;
    private readonly JwtService _jwtService;
    private readonly RoleRepository _roleRepository;

    public AuthController(AuthService authService, JwtService jwtService, RoleRepository roleRepository)
    {
        _authService = authService;
        _jwtService = jwtService;
        _roleRepository = roleRepository;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var user = await _authService.RegisterAsync(
                request.Name,
                request.Email,
                request.Password,
                CitizenRoleId,
                request.PhoneNumber);

            var response = new
            {
                id = user.Id,
                name = user.Name,
                email = user.Email,
                phone_number = user.PhoneNumber,
                role_id = user.RoleId,
                is_active = user.IsActive,
                inserted_at = user.Audit.InsertedAt
            };

            return Created(string.Empty, response);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var user = await _authService.LoginAsync(request.Email, request.Password);
            var token = await _jwtService.GenerateTokenAsync(user);

            var response = new
            {
                token,
                user = new
                {
                    id = user.Id,
                    name = user.Name,
                    email = user.Email,
                    phone_number = user.PhoneNumber,
                    role_id = user.RoleId,
                    is_active = user.IsActive
                }
            };

            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var email = User.FindFirstValue(ClaimTypes.Email);
        var role = User.FindFirstValue(ClaimTypes.Role);
        var roleId = User.FindFirstValue("role_id");

        var response = new
        {
            id,
            email,
            role,
            role_id = roleId
        };

        return Ok(response);
    }

    // TODO: remove this endpoint before deploying to production; local/testing use only
    [HttpPost("create-test-admin")]
    public async Task<IActionResult> CreateTestAdmin()
    {
        const string testAdminEmail = "admin@smartcity.local";

        var adminRole = await _roleRepository.GetByNameAsync("Admin");
        if (adminRole is null || string.IsNullOrEmpty(adminRole.Id))
        {
            return StatusCode(500, new { message = "Admin role not found." });
        }

        try
        {
            var user = await _authService.RegisterAsync(
                "Test Admin",
                testAdminEmail,
                "Admin123!",
                adminRole.Id,
                null);

            var response = new
            {
                id = user.Id,
                name = user.Name,
                email = user.Email,
                role_id = user.RoleId,
                is_active = user.IsActive
            };

            return Created(string.Empty, response);
        }
        catch (InvalidOperationException)
        {
            return Conflict(new { message = "A user with this email already exists." });
        }
    }
}
