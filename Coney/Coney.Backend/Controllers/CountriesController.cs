using Coney.Backend.Data;
using Coney.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coney.Backend.Controllers;

[ApiController]
// [Route("api/countries")]
[Route("api/[controller]")]
public class CountriesController : ControllerBase
{
    private readonly DataContext _context;

    public CountriesController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var successResponse = new ApiResponse<List<Country>>(true, 200, await _context.Countries.ToListAsync());
        return Ok(successResponse);
    }

    [HttpGet("{id}")]
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

    [HttpPost]
    public async Task<IActionResult> PostAsync(Country country)
    {
        _context.Add(country);
        await _context.SaveChangesAsync();
        return Ok(country);
    }

    [HttpPut]
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

    [HttpDelete("{id}")]
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