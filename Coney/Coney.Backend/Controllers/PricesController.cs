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
    public async Task<IActionResult> PostAsync(Prize price)
    {
        _context.Add(price);
        await _context.SaveChangesAsync();
        return Ok(price);
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        return Ok(await _context.Prices.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var rule = await _context.Prices.FindAsync(id);
        if (rule == null)
        {
            return NotFound();
        }
        return Ok(rule);
    }

    [HttpPut]
    public async Task<IActionResult> PutAsync(Prize price)
    {
        var currentPrize = await _context.Prices.FindAsync(price.Id);
        if (currentPrize == null)
        {
            return NotFound();
        }
        currentPrize.Name = price.Name;
        currentPrize.Description = price.Description;
        currentPrize.Value = price.Value;
        currentPrize.DeliveredDate = price.DeliveredDate;
        currentPrize.Delivered = price.Delivered;

        _context.Update(currentPrize);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var price = await _context.Prices.FindAsync(id);
        if (price == null)
        {
            return NotFound();
        }
        _context.Remove(price);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}