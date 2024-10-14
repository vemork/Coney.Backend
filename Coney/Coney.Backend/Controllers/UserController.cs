using Microsoft.AspNetCore.Mvc;
using Coney.Backend.DTOs;
using Coney.Backend.Services;
using System.Runtime.InteropServices;
using Coney.Backend.Repositories;

namespace Coney.Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("createUser")]
    public async Task<IActionResult> PostAsync(UserRegistrationDto userRegistrationDto)
    {
        try
        {
            var user = new UserRegistrationDto
            {
                Email = userRegistrationDto.Email,
                FirstName = userRegistrationDto.FirstName,
                LastName = userRegistrationDto.LastName,
                Password = userRegistrationDto.Password = BCrypt.Net.BCrypt.HashPassword(userRegistrationDto.Password)
            };

            await _userService.AddUserAsync(user);

            var successResponse = new ApiResponse<UserRegistrationDto>(true, 201, user);
            return Ok(successResponse);
        }
        catch (Exception ex)
        {
            var sqlException = new ApiResponse<List<object>>(false, 404, new List<object> { "Unexpected error creating record..." });
            return Conflict(sqlException);
        }
    }

    [HttpGet("verifyUser")]
    public async Task<IActionResult> Get([FromQuery] string userEmail)
    {
        try
        {
            var wasValitadionOK = await _userService.ValidateUserEmailAsync(userEmail);

            if (!wasValitadionOK)
            {
                var notFoundResponse = new ApiResponse<List<object>>(false, 404, new List<object> { "User not found" });
                return NotFound(notFoundResponse);
            }
            var verificationOk = new ApiResponse<List<object>>(true, 200, new List<object> { " Email verification was successful..." });
            return Ok(verificationOk);
        }
        catch (Exception ex)
        {
            var internalException = new ApiResponse<List<object>>(false, 503, new List<object> { "Unexpected error verifying email..." });
            return Conflict(internalException);
        }
    }

    [HttpPost("sendEmail/{email}")]
    public async Task<IActionResult> PostAsync(string email)
    {
        try
        {
            await _userService.SendEmailAsync(email);
            var successResponse = new ApiResponse<string>(true, 201, "The confirmation mail has beeen sent successfully.");
            return Ok(successResponse);
        }
        catch (Exception ex)
        {
            var sqlException = new ApiResponse<List<object>>(false, 404, new List<object> { "Unexpected error creating record..." });
            return Conflict(sqlException);
        }
    }

    /*
    [HttpGet("getAllUsers")]
    public async Task<IActionResult> GetAsync()
    {
        var successResponse = new ApiResponse<IEnumerable<User>>(true, 200, await _userService.GetAllAsync());
        return Ok(successResponse);
    }

    [HttpGet("getUser/{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, new List<object> { "User not found" });
            return Ok(NotFoundResponse);
        }
        var successResponse = new ApiResponse<User>(true, 200, user);
        return Ok(successResponse);
    }

    [HttpPut("updateUser/{id}")]
    public async Task<IActionResult> PutAsync(int id, UpdateUserDto userDto)
    {
        try
        {
            var currentUser = await _userRepository.FindUserAsync(id);
            if (currentUser == null)
            {
                var notFoundResponse = new ApiResponse<List<object>>(false, 404, new List<object> { "User not found" });
                return NotFound(notFoundResponse);
            }

            if (!string.IsNullOrEmpty(userDto.Password))
            {
                currentUser.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            }

            currentUser.FirstName = userDto.FirstName ?? currentUser.FirstName;
            currentUser.LastName = userDto.LastName ?? currentUser.LastName;
            currentUser.UpdatedAt = DateTime.Now;

            await _userRepository.UpdateAsync(currentUser);
            var successResponse = new ApiResponse<List<User>>(true, 200, new List<User> { currentUser });
            return Ok(successResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpDelete("deleteUser/{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            var user = await _userRepository.FindUserAsync(id);
            if (user == null)
            {
                var NotFoundResponse = new ApiResponse<List<object>>(false, 404, new List<object> { "User not found" });
                return Ok(NotFoundResponse);
            }
            await _userRepository.DeleteAsync(user);
            var successResponse = new ApiResponse<List<User>>(true, 200, []);
            return Ok(successResponse);
        }
        catch (Exception ex)
        {
            var sqlException = new ApiResponse<List<object>>(false, 404, new List<object> { "Unexpected error deleting record..." });
            return Conflict(sqlException);
        }
    }
    */
}