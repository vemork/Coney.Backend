using Coney.Backend.Data;
using Coney.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Coney.Backend.DTOs;

namespace Coney.Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly DataContext _context;

    public UsersController(DataContext context)
    {
        _context = context;
    }

    [HttpPost("createUser")]
    public async Task<IActionResult> PostAsync(User user)
    {
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        _context.Add(user);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<User>(true, 201, user);
        return Ok(successResponse);
    }

    [HttpGet("getAllUsers")]
    public async Task<IActionResult> GetAsync()
    {
        var successResponse = new ApiResponse<List<User>>(true, 200, await _context.Users.ToListAsync());
        return Ok(successResponse);
    }

    [HttpGet("getUser/{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        var successResponse = new ApiResponse<User>(true, 200, user);
        return Ok(successResponse);
    }

    [HttpPut("updateUser/{id}")]    
    public async Task<IActionResult> PutAsync(int id,UpdateUserDto userDto)
    {
        try
        {
            var currentUser = await _context.Users.FindAsync(id);
            if (currentUser == null)
            {
                var notFoundResponse = new ApiResponse<List<object>>(false, 404, new List<object> { "Usuario no encontrado" });
                return NotFound(notFoundResponse);
            }

            if (!string.IsNullOrEmpty(userDto.Password))
            {
                currentUser.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            }

            currentUser.FirstName = userDto.FirstName ?? currentUser.FirstName ;
            currentUser.LastName = userDto.LastName ?? currentUser.LastName;
            currentUser.UpdatedAt = DateTime.Now;

            _context.Update(currentUser);
            await _context.SaveChangesAsync();

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
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        _context.Remove(user);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<List<User>>(true, 200, []);
        return Ok(successResponse);
    }
}