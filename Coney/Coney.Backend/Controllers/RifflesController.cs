using Coney.Backend.Data;
using Coney.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coney.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RifflesController : ControllerBase
{
    private readonly DataContext _context;

    public RifflesController(DataContext context)
    {
        _context = context;
    }

    [HttpPost("createRiffle")]
    public async Task<IActionResult> PostAsync(Riffle riffle)
    {
        _context.Add(riffle);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<Riffle>(true, 200, riffle);
        return Ok(successResponse);
    }

    [HttpGet("getAllRiffles")]
    public async Task<IActionResult> GetAsync()
    {
        var successResponse = new ApiResponse<List<Riffle>>(true, 200, await _context.Riffles.ToListAsync());
        return Ok(successResponse);
    }

    [HttpGet("getRiffle/{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var riffle = await _context.Riffles.FindAsync(id);
        if (riffle == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        var successResponse = new ApiResponse<Riffle>(true, 200, riffle);
        return Ok(successResponse);
    }

    [HttpPut("updateRiffle")]
    public async Task<IActionResult> PutAsync(Riffle riffle)
    {
        var currentRiffle = await _context.Riffles.FindAsync(riffle.Id);
        if (currentRiffle == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        currentRiffle.Name = riffle.Name;
        currentRiffle.Description = riffle.Description;
        currentRiffle.InitDate = riffle.InitDate;
        currentRiffle.EndtDate = riffle.EndtDate;
        currentRiffle.Status = riffle.Status;
        currentRiffle.amountTickets = riffle.amountTickets;
        currentRiffle.amountBusyTickets = riffle.amountBusyTickets;
        _context.Update(currentRiffle);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<List<Riffle>>(true, 200, []);
        return Ok(successResponse);
    }

    [HttpDelete("deleteRiffle/{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var riffle = await _context.Riffles.FindAsync(id);
        if (riffle == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        _context.Remove(riffle);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<List<Riffle>>(true, 200, []);
        return Ok(successResponse);
    }
}