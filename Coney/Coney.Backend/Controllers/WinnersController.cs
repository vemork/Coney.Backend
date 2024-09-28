using Coney.Backend.Data;
using Coney.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coney.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WinnersController : ControllerBase
{
    private readonly DataContext _context;

    public WinnersController(DataContext context)
    {
        _context = context;
    }

    [HttpPost("createWinner")]
    public async Task<IActionResult> PostAsync(Winner winner)
    {
        _context.Add(winner);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<Winner>(true, 200, winner);
        return Ok(successResponse);
    }

    [HttpGet("getAllWinners")]
    public async Task<IActionResult> GetAsync()
    {
        var successResponse = new ApiResponse<List<Winner>>(true, 200, await _context.Winners.ToListAsync());
        return Ok(successResponse);
    }

    [HttpGet("getWinner/{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var winner = await _context.Winners.FindAsync(id);
        if (winner == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        var successResponse = new ApiResponse<Winner>(true, 200, winner);
        return Ok(successResponse);
    }

    [HttpPut("updateWinner")]
    public async Task<IActionResult> PutAsync(Winner winner)
    {
        var currentWinner = await _context.Winners.FindAsync(winner.Id);
        if (currentWinner == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        currentWinner.Observations = winner.Observations;
        currentWinner.WasDelivered = winner.WasDelivered;
        currentWinner.PrizeId = winner.PrizeId;
        currentWinner.UserId = winner.UserId;
        currentWinner.RiffleId = winner.RiffleId;
        _context.Update(currentWinner);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<List<Winner>>(true, 200, []);
        return Ok(successResponse);
    }

    [HttpDelete("deleteWinner/{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var winner = await _context.Winners.FindAsync(id);
        if (winner == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        _context.Remove(winner);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<List<Winner>>(true, 200, []);
        return Ok(successResponse);
    }
}