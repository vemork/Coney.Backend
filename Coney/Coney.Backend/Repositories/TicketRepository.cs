using Coney.Backend.Data;
using Coney.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coney.Backend.Repositories;

public class TicketRepository
{
    private readonly DataContext _context;

    public TicketRepository(DataContext context)
    {
        _context = context;
    }

    // returns all the information from the users
    public async Task<IEnumerable<Ticket>> GetAllAsync()
    {
        return await _context.Tickets.ToListAsync();
    }

    // returns the information of a single ticket using the id
    public async Task<Ticket?> GetByIdAsync(int id)
    {
        return await _context.Tickets.FindAsync(id);
    }

    // Save a ticket information
    public async Task AddAsync(Ticket ticket)
    {
        await _context.Tickets.AddAsync(ticket);
        await _context.SaveChangesAsync();
    }

    // Update ticket information
    public async Task UpdateAsync(Ticket ticket)
    {
        //_context.Entry(ticket).State = EntityState.Modified;
        _context.Tickets.Update(ticket);
        await _context.SaveChangesAsync();
    }

    // Delete the information of a single ticket using the id
    public async Task DeleteAsync(Ticket ticket)
    {
        _context.Tickets.Remove(ticket);
        await _context.SaveChangesAsync();
    }

    // returns the information about Ticket
    public async Task<Ticket?> FindTicketAsync(int id)
    {
        return await _context.Tickets.FindAsync(id);
    }
}