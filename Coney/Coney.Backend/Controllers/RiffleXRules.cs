using Coney.Backend.Data;
using Coney.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coney.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RiffleXRulesController : ControllerBase
{
    private readonly DataContext _context;

    public RiffleXRulesController(DataContext context)
    {
        _context = context;
    }

    [HttpPost("createRiffleXRule")]
    public async Task<IActionResult> PostAsync(RiffleXRule riffleXrule)
    {
        _context.Add(riffleXrule);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<RiffleXRule>(true, 200, riffleXrule);
        return Ok(successResponse);
    }

    [HttpGet("getAllRiffleXRules")]
    public async Task<IActionResult> GetAsync()
    {
        var successResponse = new ApiResponse<List<RiffleXRule>>(true, 200, await _context.RiffleXRules.ToListAsync());
        return Ok(successResponse);
    }

    [HttpGet("getRiffleXRule/{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var riffleXrule = await _context.RiffleXRules.FindAsync(id);
        if (riffleXrule == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        var successResponse = new ApiResponse<RiffleXRule>(true, 200, riffleXrule);
        return Ok(successResponse);
    }

    [HttpPut("updateRiffleXRule")]
    public async Task<IActionResult> PutAsync(RiffleXRule riffleXrule)
    {
        var currentRiffleXRule = await _context.RiffleXRules.FindAsync(riffleXrule.Id);
        if (currentRiffleXRule == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        currentRiffleXRule.RiffleId = riffleXrule.RiffleId;
        currentRiffleXRule.RuleId = riffleXrule.RuleId;
        _context.Update(currentRiffleXRule);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<List<RiffleXRule>>(true, 200, []);
        return Ok(successResponse);
    }

    [HttpDelete("deleteRiffleXRule/{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var riffleXrule = await _context.RiffleXRules.FindAsync(id);
        if (riffleXrule == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        _context.Remove(riffleXrule);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<List<RiffleXRule>>(true, 200, []);
        return Ok(successResponse);
    }
}