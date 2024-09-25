using Coney.Backend.Data;
using Coney.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coney.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PricesController : ControllerBase
{
    private readonly DataContext _context;

    public PricesController(DataContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(Prize prize)
    {
        _context.Add(prize);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<Prize>(true, 200, prize);
        return Ok(successResponse);
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var successResponse = new ApiResponse<List<Prize>>(true, 200, await _context.Prices.ToListAsync());
        return Ok(successResponse);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var prize = await _context.Prices.FindAsync(id);
        if (prize == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        var successResponse = new ApiResponse<Prize>(true, 200, prize);
        return Ok(successResponse);
    }

    [HttpPut]
    public async Task<IActionResult> PutAsync(Prize prize)
    {
        var currentPrize = await _context.Prices.FindAsync(prize.Id);
        if (currentPrize == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        currentPrize.Name = prize.Name;
        currentPrize.Description = prize.Description;
        currentPrize.Value = prize.Value;
        currentPrize.DeliveredDate = prize.DeliveredDate;
        currentPrize.Delivered = prize.Delivered;

        _context.Update(currentPrize);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<Prize>(true, 200, currentPrize);
        return Ok(successResponse);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var prize = await _context.Prices.FindAsync(id);
        if (prize == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        _context.Remove(prize);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<List<Prize>>(true, 200, []);
        return Ok(successResponse);
    }
}