using Coney.Backend.DTOs;
using Coney.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Coney.Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthsController : ControllerBase

{
    private readonly AuthService _authService;
    private readonly UserService _userService;

    public AuthsController(AuthService authService, UserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            var user = await _userService.GetUserByEmailAsync(request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                var unauthorizeResponse = new ApiResponse<List<object>>(false, 401, new List<object> { "Unexpected error searching user..." });
                return Unauthorized(unauthorizeResponse);
            }

            var token = _authService.GenerateJwtToken(user);
            var tokenizedUser = new UserResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
            var authorizeResponse = new ApiResponse<object>(true, 200, new { user = tokenizedUser, token });
            return Ok(authorizeResponse);
        }
        catch (Exception ex)
        {
            var sqlException = new ApiResponse<List<object>>(false, 404, new List<object> { "User not found..." });
            return NotFound(sqlException);
        }
    }
}