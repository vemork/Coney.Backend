using Coney.Backend.Data;
using Coney.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coney.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatesController : ControllerBase
{
    private readonly DataContext _context;

    public StatesController(DataContext context)
    {
        _context = context;
    }

    [HttpPost("createState")]
    public async Task<IActionResult> PostAsync(State state)
    {
        _context.Add(state);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<State>(true, 200, state);
        return Ok(successResponse);
    }

    [HttpGet("getAllStates")]
    public async Task<IActionResult> GetAsync()
    {
        var successResponse = new ApiResponse<List<State>>(true, 200, await _context.States.ToListAsync());
        return Ok(successResponse);
    }

    [HttpGet("getState/{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var state = await _context.States.FindAsync(id);
        if (state == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        var successResponse = new ApiResponse<State>(true, 200, state);
        return Ok(successResponse);
    }

    [HttpPut("updateState")]
    public async Task<IActionResult> PutAsync(State state)
    {
        var currentState = await _context.States.FindAsync(state.Id);
        if (currentState == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        currentState.Name = state.Name;
        currentState.CountryId = state.CountryId;
        _context.Update(currentState);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<List<State>>(true, 200, []);
        return Ok(successResponse);
    }

    

    [HttpDelete("deleteState/{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var state = await _context.States.FindAsync(id);
        if (state == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        _context.Remove(state);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<List<State>>(true, 200, []);
        return Ok(successResponse);
    }
}