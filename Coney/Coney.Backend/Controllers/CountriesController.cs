using Coney.Backend.Data;
using Coney.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coney.Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountriesController : ControllerBase
{
    private readonly DataContext _context;

    public CountriesController(DataContext context)
    {
        _context = context;
    }

    [HttpPost("createCountry")]
    public async Task<IActionResult> PostAsync(Country country)
    {
        _context.Add(country);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<Country>(true, 200, country);
        return Ok(successResponse);
    }

    [HttpGet("getAllCountries")]
    public async Task<IActionResult> GetAsync()
    {
        var successResponse = new ApiResponse<List<Country>>(true, 200, await _context.Countries.ToListAsync());
        return Ok(successResponse);
    }

    [HttpGet("getCountry/{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var country = await _context.Countries.FindAsync(id);
        if (country == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        var successResponse = new ApiResponse<Country>(true, 200, country);
        return Ok(successResponse);
    }

    [HttpPut("updateCountry")]
    public async Task<IActionResult> PutAsync(Country country)
    {
        var currentCountry = await _context.Countries.FindAsync(country.Id);
        if (currentCountry == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        currentCountry.Name = country.Name;
        _context.Update(currentCountry);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<List<Country>>(true, 200, []);
        return Ok(successResponse);
    }

    [HttpDelete("deleteCountry/{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var country = await _context.Countries.FindAsync(id);
        if (country == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        _context.Remove(country);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<List<Country>>(true, 200, []);
        return Ok(successResponse);
    }
}