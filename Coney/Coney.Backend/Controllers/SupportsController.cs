using Coney.Backend.Data;
using Coney.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coney.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SupportsController : ControllerBase
{
    private readonly DataContext _context;

    public SupportsController(DataContext context)
    {
        _context = context;
    }

    [HttpPost("createSupport")]
    public async Task<IActionResult> PostAsync(Support support)
    {
        _context.Add(support);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<Support>(true, 200, support);
        return Ok(successResponse);
    }

    [HttpGet("getAllSupports")]
    public async Task<IActionResult> GetAsync()
    {
        var successResponse = new ApiResponse<List<Support>>(true, 200, await _context.Supports.ToListAsync());
        return Ok(successResponse);
    }

    [HttpGet("getSupport/{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var support = await _context.Supports.FindAsync(id);
        if (support == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        var successResponse = new ApiResponse<Support>(true, 200, support);
        return Ok(successResponse);
    }

    [HttpPut("updateSupport")]
    public async Task<IActionResult> PutAsync(Support support)
    {
        var currentSupport = await _context.Supports.FindAsync(support.Id);
        if (currentSupport == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        currentSupport.IsResolved = support.IsResolved;
        currentSupport.Description = support.Description;
        currentSupport.ResolvedAt = support.ResolvedAt;
        currentSupport.PersonInCharge = support.PersonInCharge;
        _context.Update(currentSupport);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<List<Support>>(true, 200, []);
        return Ok(successResponse);
    }

    [HttpDelete("deleteSupport/{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var support = await _context.Supports.FindAsync(id);
        if (support == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        _context.Remove(support);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<List<Support>>(true, 200, []);
        return Ok(successResponse);
    }
}