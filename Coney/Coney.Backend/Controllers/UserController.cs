using Coney.Backend.DTOs;
using Coney.Backend.Services;
using Coney.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

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
                Password = userRegistrationDto.Password
            };

            var recordedUser = await _userService.AddUserAsync(user);

            var successResponse = new ApiResponse<UserRegistrationDto>(true, 201, recordedUser);
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
            var internalException = new ApiResponse<List<object>>(false, 503, new List<object> { "Unexpected error verifying user email..." });
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
            var sqlException = new ApiResponse<List<object>>(false, 404, new List<object> { "Unexpected error sending email verification ..." });
            return Conflict(sqlException);
        }
    }

    [HttpPost("adminVerification/{email}")]
    public async Task<IActionResult> PostAdminVerification(string email)
    {
        try
        {
            var data = await _userService.UpdateUserAdminVerificationAsync(email);

            var successResponse = new ApiResponse<object>(true, 201, new List<object?> { data });
            return Ok(successResponse);
        }
        catch (Exception ex)
        {
            var sqlException = new ApiResponse<List<object>>(false, 404, new List<object> { "Unexpected error when admin try to verify a user email record ..." });
            return Conflict(sqlException);
        }
    }

    [HttpGet("getAllUsers")]
    public async Task<IActionResult> GetAsync()
    {
        var successResponse = new ApiResponse<IEnumerable<User>>(true, 200, await _userService.GetAllUsersAsync());
        return Ok(successResponse);
    }

    [HttpGet("getUser/{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                var NotFoundResponse = new ApiResponse<List<object>>(false, 404, new List<object> { "User not found" });
                return Ok(NotFoundResponse);
            }
            var successResponse = new ApiResponse<List<User>>(true, 200, new List<User> { user });
            return Ok(successResponse);
        }
        catch (Exception ex)
        {
            var sqlException = new ApiResponse<List<object>>(false, 404, new List<object> { "Unexpected error trying to get user ..." + ex });
            return Conflict(sqlException);
        }
    }

    [HttpPut("updateUser/{id}")]
    public async Task<IActionResult> PutAsync(int id, UserUpdateDto userDto)
    {
        try
        {
            var userResponse = await _userService.UpdateUserAsync(id, userDto);
            var successResponse = new ApiResponse<List<UserResponseDto>>(true, 200, new List<UserResponseDto> { userResponse });
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
            await _userService.DeleteUserAsync(id);
            var successResponse = new ApiResponse<List<User>>(true, 200, []);
            return Ok(successResponse);
        }
        catch (Exception ex)
        {
            var sqlException = new ApiResponse<List<object>>(false, 404, new List<object> { "Unexpected error deleting record..." });
            return Conflict(sqlException);
        }
    }
}