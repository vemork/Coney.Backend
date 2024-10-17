using Coney.Backend.DTOs;
using Coney.Backend.Services;
using Coney.Shared.Entities;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Coney.Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthsController : ControllerBase

{
    private readonly AuthService _authService;
    private readonly UserService _userService;
    private readonly JwtSettings _jwtSettings;

    public AuthsController(AuthService authService, UserService userService, IOptions<JwtSettings> jwtSettings)
    {
        _authService = authService;
        _userService = userService;
        _jwtSettings = jwtSettings.Value;
    }

    private string GenerateJwtToken(User user)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, "1234567890"),
            new Claim("id", user.Id.ToString()),
            new Claim("firstName", user.FirstName),
            new Claim("lastName", user.LastName),
            new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("role", user.Role)
        };

        var tokenOptions = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(double.Parse(_jwtSettings.ExpiresInMinutes)),
            signingCredentials: signinCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
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

            var token = GenerateJwtToken(user);

            var authorizeResponse = new ApiResponse<object>(true, 200, new { token });
            return Ok(authorizeResponse);
        }
        catch (Exception ex)
        {
            var sqlException = new ApiResponse<List<object>>(false, 404, new List<object> { "User not found..." });
            return NotFound(sqlException);
        }
    }
}