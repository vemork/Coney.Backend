using Coney.Backend.Data;
using Coney.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coney.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RulesController : ControllerBase
{
    private readonly DataContext _context;

    public RulesController(DataContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(Rule rule)
    {
        _context.Add(rule);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<Rule>(true, 200, rule);
        return Ok(successResponse);
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var successResponse = new ApiResponse<List<Rule>>(true, 200, await _context.Rules.ToListAsync());
        return Ok(successResponse);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var rule = await _context.Rules.FindAsync(id);
        if (rule == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        var successResponse = new ApiResponse<Rule>(true, 200, rule);
        return Ok(successResponse);
    }

    [HttpPut]
    public async Task<IActionResult> PutAsync(Rule rule)
    {
        var currentRule = await _context.Rules.FindAsync(rule.Id);
        if (currentRule == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        currentRule.Description = rule.Description;
        currentRule.InitDate = rule.InitDate;
        currentRule.EndtDate = rule.EndtDate;
        currentRule.Status = rule.Status;

        _context.Update(currentRule);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<Rule>(true, 200, currentRule);
        return Ok(successResponse);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var rule = await _context.Rules.FindAsync(id);
        if (rule == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        _context.Remove(rule);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<List<Rule>>(true, 200, []);
        return Ok(successResponse);
    }
}