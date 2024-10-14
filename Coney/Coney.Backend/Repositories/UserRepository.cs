using Coney.Backend.Data;
using Coney.Shared.Entities;
using Coney.Backend.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Coney.Backend.Repositories;

public class UserRepository
{
    private readonly DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context;
    }

    // returns all the information from the users
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    // returns the information of a single user using the id
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    // Save a user information
    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    // Update user information
    public async Task UpdateAsync(User user)
    {
        //_context.Entry(user).State = EntityState.Modified;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    // Delete the information of a single user using the id
    public async Task DeleteAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    // returns the information of a single user using the email
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    // returns the information about User
    public async Task<User?> FindUserAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }
}