using Coney.Backend.Data;
using Coney.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coney.Backend.Repositories;

public class RiffleRepository
{
    private readonly DataContext _context;

    public RiffleRepository(DataContext context)
    {
        _context = context;
    }

    // returns all the information from the countries
    public async Task<IEnumerable<Riffle>> GetAllAsync()
    {
        return await _context.Riffles.ToListAsync();
    }

    // returns the information of a single riffle using the id
    public async Task<Riffle?> GetByIdAsync(int id)
    {
        return await _context.Riffles.FindAsync(id);
    }

    // Save a riffle information
    public async Task AddAsync(Riffle riffle)
    {
        await _context.Riffles.AddAsync(riffle);
        await _context.SaveChangesAsync();
    }

    // Update riffle information
    public async Task UpdateAsync(Riffle riffle)
    {
        //_context.Entry(riffle).State = EntityState.Modified;
        _context.Riffles.Update(riffle);
        await _context.SaveChangesAsync();
    }

    // Delete the information of a single riffle using the id
    public async Task DeleteAsync(Riffle riffle)
    {
        await _context.SaveChangesAsync();
        _context.Riffles.Remove(riffle);
        await _context.SaveChangesAsync();
    }

    // returns the information about Riffle
    public async Task<Riffle?> FindRiffleAsync(int id)
    {
        return await _context.Riffles.FindAsync(id);
    }
}