using Coney.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Coney.Backend.Repositories;
using Coney.Backend.DTOs;

namespace Coney.Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserRepository _userRepository;

    public UsersController(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost("createUser")]
    public async Task<IActionResult> PostAsync(User user)
    {
        try
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            await _userRepository.AddAsync(user);
            var successResponse = new ApiResponse<User>(true, 201, user);
            return Ok(successResponse);
        }
        catch (Exception ex)
        {
            var sqlException = new ApiResponse<List<object>>(false, 404, new List<object> { "Unexpected error creating record..." });
            return Conflict(sqlException);
        }
    }

    [HttpGet("getAllUsers")]
    public async Task<IActionResult> GetAsync()
    {
        var successResponse = new ApiResponse<IEnumerable<User>>(true, 200, await _userRepository.GetAllAsync());
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
}