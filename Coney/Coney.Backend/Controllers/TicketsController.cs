using Coney.Backend.Data;
using Coney.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coney.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly DataContext _context;

    public TicketsController(DataContext context)
    {
        _context = context;
    }

    [HttpPost("createTicket")]
    public async Task<IActionResult> PostAsync(Ticket ticket)
    {
        _context.Add(ticket);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<Ticket>(true, 200, ticket);
        return Ok(successResponse);
    }

    [HttpGet("getAllTickets")]
    public async Task<IActionResult> GetAsync()
    {
        var successResponse = new ApiResponse<List<Ticket>>(true, 200, await _context.Tickets.ToListAsync());
        return Ok(successResponse);
    }

    [HttpGet("getTicket/{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        var successResponse = new ApiResponse<Ticket>(true, 200, ticket);
        return Ok(successResponse);
    }

    [HttpPut("updateTicket")]
    public async Task<IActionResult> PutAsync(Ticket ticket)
    {
        var currentTicket = await _context.Tickets.FindAsync(ticket.Id);
        if (currentTicket == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        currentTicket.Code = ticket.Code;
        currentTicket.WasPaid = ticket.WasPaid;
        currentTicket.RiffleId = ticket.RiffleId;
        _context.Update(currentTicket);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<List<Ticket>>(true, 200, []);
        return Ok(successResponse);
    }

    [HttpDelete("deleteTicket/{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        _context.Remove(ticket);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<List<Ticket>>(true, 200, []);
        return Ok(successResponse);
    }
}