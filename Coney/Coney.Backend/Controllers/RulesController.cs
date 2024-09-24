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

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        return Ok(await _context.Rules.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var rule = await _context.Rules.FindAsync(id);
        if (rule == null)
        {
            return NotFound();
        }
        return Ok(rule);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(Rule rule)
    {
        _context.Add(rule);
        await _context.SaveChangesAsync();
        return Ok(rule);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var rule = await _context.Rules.FindAsync(id);
        if (rule == null)
        {
            return NotFound();
        }
        _context.Remove(rule);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> PutAsync(Rule rule)
    {
        var currentRule = await _context.Rules.FindAsync(rule.Id);
        if (currentRule == null)
        {
            return NotFound();
        }
        currentRule.Description = rule.Description;
        currentRule.InitDate = rule.InitDate;
        currentRule.EndtDate = rule.EndtDate;
        currentRule.Status = rule.Status;

        _context.Update(currentRule);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}