using Coney.Backend.Repositories;
using Coney.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Coney.Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountriesController : ControllerBase
{
    private readonly CountryRepository _countryRepository;

    public CountriesController(CountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }

    [HttpPost("createCountry")]
    public async Task<IActionResult> PostAsync(Country country)
    {
        try
        {
            await _countryRepository.AddAsync(country);
            var successResponse = new ApiResponse<Country>(true, 201, country);
            return Ok(successResponse);
        }
        catch (Exception ex)
        {
            var sqlException = new ApiResponse<List<object>>(false, 404, new List<object> { "Unexpected error creating record..." });
            return Conflict(sqlException);
        }
    }

    [HttpGet("getAllCountries")]
    public async Task<IActionResult> GetAsync()
    {
        var successResponse = new ApiResponse<IEnumerable<Country>>(true, 200, await _countryRepository.GetAllAsync());
        return Ok(successResponse);
    }

    [HttpGet("getCountry/{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var country = await _countryRepository.GetByIdAsync(id);
        if (country == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, new List<object> { "Country not found" });
            return Ok(NotFoundResponse);
        }
        var successResponse = new ApiResponse<Country>(true, 200, country);
        return Ok(successResponse);
    }

    [HttpPut("updateCountry/{id}")]
    public async Task<IActionResult> PutAsync(int id, Country country)
    {
        try
        {
            var currentCountry = await _countryRepository.FindCountryAsync(id);
            if (currentCountry == null)
            {
                var NotFoundResponse = new ApiResponse<List<object>>(false, 404, new List<object> { "Country not found" });
                return Ok(NotFoundResponse);
            }
            currentCountry.Name = country.Name;
            await _countryRepository.UpdateAsync(currentCountry);
            var successResponse = new ApiResponse<List<Country>>(true, 200, new List<Country> { currentCountry });
            return Ok(successResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpDelete("deleteCountry/{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            var country = await _countryRepository.FindCountryAsync(id);
            if (country == null)
            {
                var NotFoundResponse = new ApiResponse<List<object>>(false, 404, new List<object> { "Country not found" });
                return Ok(NotFoundResponse);
            }
            await _countryRepository.DeleteAsync(country);
            var successResponse = new ApiResponse<List<Country>>(true, 200, []);
            return Ok(successResponse);
        }
        catch (Exception ex)
        {
            var sqlException = new ApiResponse<List<object>>(false, 404, new List<object> { "Unexpected error deleting record..." });
            return Conflict(sqlException);
        }
    }
}