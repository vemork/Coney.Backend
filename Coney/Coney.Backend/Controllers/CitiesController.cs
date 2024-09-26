using Coney.Backend.Data;
using Coney.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coney.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CitiesController : ControllerBase
{
    private readonly DataContext _context;

    public CitiesController(DataContext context)
    {
        _context = context;
    }

    [HttpPost("createCity")]
    public async Task<IActionResult> PostAsync(City city)
    {
        _context.Add(city);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<City>(true, 200, city);
        return Ok(successResponse);
    }

    [HttpGet("getAllCities")]
    public async Task<IActionResult> GetAsync()
    {
        var successResponse = new ApiResponse<List<City>>(true, 200, await _context.Cities.ToListAsync());
        return Ok(successResponse);
    }

    [HttpGet("getCity/{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var city = await _context.Cities.FindAsync(id);
        if (city == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        var successResponse = new ApiResponse<City>(true, 200, city);
        return Ok(successResponse);
    }

    [HttpPut("updateCity")]
    public async Task<IActionResult> PutAsync(City city)
    {
        var currentCity = await _context.Cities.FindAsync(city.Id);
        if (currentCity == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        currentCity.Name = city.Name;
        currentCity.StateId = city.StateId;
        _context.Update(currentCity);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<List<City>>(true, 200, []);
        return Ok(successResponse);
    }

    [HttpDelete("deleteCity/{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var city = await _context.Cities.FindAsync(id);
        if (city == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        _context.Remove(city);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<List<City>>(true, 200, []);
        return Ok(successResponse);
    }
}