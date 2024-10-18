using Coney.Backend.Data;
using Coney.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coney.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserXRifflesController : ControllerBase
{
    private readonly DataContext _context;

    public UserXRifflesController(DataContext context)
    {
        _context = context;
    }

    [HttpPost("createUserXRiffle")]
    public async Task<IActionResult> PostAsync(UserXRiffle userXriffle)
    {
        _context.Add(userXriffle);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<UserXRiffle>(true, 200, userXriffle);
        return Ok(successResponse);
    }

    [HttpGet("getAllUserXRiffles")]
    public async Task<IActionResult> GetAsync()
    {
        var successResponse = new ApiResponse<List<UserXRiffle>>(true, 200, await _context.UserXRiffles.ToListAsync());
        return Ok(successResponse);
    }

    [HttpGet("getUserXRiffle/{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var userXriffle = await _context.UserXRiffles.FindAsync(id);
        if (userXriffle == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        var successResponse = new ApiResponse<UserXRiffle>(true, 200, userXriffle);
        return Ok(successResponse);
    }

    [HttpPut("updateUserXRiffle")]
    public async Task<IActionResult> PutAsync(UserXRiffle userXriffle)
    {
        var currentUserXRiffle = await _context.UserXRiffles.FindAsync(userXriffle.Id);
        if (currentUserXRiffle == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        currentUserXRiffle.UserId = userXriffle.UserId;
        currentUserXRiffle.RiffleId = userXriffle.RiffleId;
        currentUserXRiffle.CreatedAt = userXriffle.CreatedAt;
        _context.Update(currentUserXRiffle);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<List<UserXRiffle>>(true, 200, []);
        return Ok(successResponse);
    }

    [HttpDelete("deleteUserXRiffle/{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var userXriffle = await _context.UserXRiffles.FindAsync(id);
        if (userXriffle == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        _context.Remove(userXriffle);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<List<UserXRiffle>>(true, 200, []);
        return Ok(successResponse);
    }
}