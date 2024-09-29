using Coney.Backend.Data;
using Coney.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coney.Backend.Repositories;

public class CountryRepository
{
    private readonly DataContext _context;

    public CountryRepository(DataContext context)
    {
        _context = context;
    }

    // returns all the information from the countries
    public async Task<IEnumerable<Country>> GetAllAsync()
    {
        return await _context.Countries.ToListAsync();
    }

    // returns the information of a single country using the id
    public async Task<Country?> GetByIdAsync(int id)
    {
        return await _context.Countries.FindAsync(id);
    }

    // Save a country information
    public async Task AddAsync(Country country)
    {
        await _context.Countries.AddAsync(country);
        await _context.SaveChangesAsync();
    }

    // Update country information
    public async Task UpdateAsync(Country country)
    {
        //_context.Entry(country).State = EntityState.Modified;
        _context.Countries.Update(country);
        await _context.SaveChangesAsync();
    }

    // Delete the information of a single country using the id
    public async Task DeleteAsync(Country country)
    {
        _context.Countries.Remove(country);
        await _context.SaveChangesAsync();
    }

    // returns the information about Country
    public async Task<Country?> FindCountryAsync(int id)
    {
        return await _context.Countries.FindAsync(id);
    }
}