using Coney.Backend.Repositories;
using Coney.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Coney.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly TicketRepository _ticketRepository;

    public TicketsController(TicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    [HttpPost("createTicket")]
    public async Task<IActionResult> PostAsync(Ticket ticket)
    {
        try
        {
            await _ticketRepository.AddAsync(ticket);
            var successResponse = new ApiResponse<Ticket>(true, 201, ticket);
            return Ok(successResponse);
        }
        catch (Exception)
        {
            var sqlException = new ApiResponse<List<object>>(false, 404, new List<object> { "Unexpected error creating record..." });
            return Conflict(sqlException);
        }
    }

    [HttpGet("getAllTickets")]
    public async Task<IActionResult> GetAsync()
    {
        var successResponse = new ApiResponse<IEnumerable<Ticket>>(true, 200, await _ticketRepository.GetAllAsync());
        return Ok(successResponse);
    }

    [HttpGet("getAllTicketsForReservation")]
    public async Task<IActionResult> getAsyncForReservation()
    {
        var successResponse = new ApiResponse<IEnumerable<Ticket>>
            (true, 200, await _ticketRepository.getAsyncForReservation());
        return Ok(successResponse);
    }

    [HttpGet("getTicket/{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);
        if (ticket == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, new List<object> { "Ticket not found" });
            return Ok(NotFoundResponse);
        }
        var successResponse = new ApiResponse<Ticket>(true, 200, ticket);
        return Ok(successResponse);
    }

    [HttpPut("updateTicket/{id}")]
    public async Task<IActionResult> PutAsync(int id, Ticket ticket)
    {
        try
        {
            var currentTicket = await _ticketRepository.FindTicketAsync(id);
            if (currentTicket == null)
            {
                var NotFoundResponse = new ApiResponse<List<object>>(false, 404, new List<object> { "Ticket not found" });
                return Ok(NotFoundResponse);
            }
            currentTicket.TicketNumber = ticket.TicketNumber;
            currentTicket.UserId = ticket.UserId;
            currentTicket.RiffleId = ticket.RiffleId;
            await _ticketRepository.UpdateAsync(currentTicket);
            var successResponse = new ApiResponse<List<Ticket>>(true, 200, new List<Ticket> { currentTicket });
            return Ok(successResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpDelete("deleteTicket/{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            var ticket = await _ticketRepository.FindTicketAsync(id);
            if (ticket == null)
            {
                var NotFoundResponse = new ApiResponse<List<object>>(false, 404, new List<object> { "Ticket not found" });
                return Ok(NotFoundResponse);
            }
            await _ticketRepository.DeleteAsync(ticket);
            var successResponse = new ApiResponse<List<Ticket>>(true, 200, []);
            return Ok(successResponse);
        }
        catch (Exception ex)
        {
            var sqlException = new ApiResponse<List<object>>(false, 404, new List<object> { "Unexpected error deleting record..." });
            return Conflict(sqlException);
        }
    }
}