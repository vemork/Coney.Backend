using Coney.Backend.Repositories;
using Coney.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Coney.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RifflesController : ControllerBase
{
    private readonly RiffleRepository _riffleRepository;
    private readonly TicketRepository _ticketRepository;

    public RifflesController(
        RiffleRepository riffleRepository,
        TicketRepository ticketRepository
    )
    {
        _riffleRepository = riffleRepository;
        _ticketRepository = ticketRepository;
    }

    [HttpPost("createRiffle")]
    public async Task<IActionResult> PostAsync(Riffle riffle)
    {
        try
        {
            await _riffleRepository.AddAsync(riffle);

            // Generate a list of 100 consecutive tickets for the created raffle
            var tickets = new List<Ticket>();
            for (int i = 1; i <= 100; i++)
            {
                tickets.Add(new Ticket
                {
                    TicketNumber = i.ToString(),
                    RiffleId = riffle.Id,
                    UserId = null
                });
            }

            // 100 tickets are inserted for each raffle
            await _ticketRepository.AddMultipleTicketsAsync(tickets);

            var successResponse = new ApiResponse<Riffle>(true, 201, riffle);
            return Ok(successResponse);
        }
        catch (Exception)
        {
            var sqlException = new ApiResponse<List<object>>(false, 404, new List<object> { "Unexpected error creating record..." });
            return Conflict(sqlException);
        }
    }

    [HttpGet("getAllRiffles")]
    public async Task<IActionResult> GetAsync()
    {
        var successResponse = new ApiResponse<IEnumerable<Riffle>>(true, 200, await _riffleRepository.GetAllAsync());
        return Ok(successResponse);
    }

    [HttpGet("getRiffle/{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        try
        {
            var riffle = await _riffleRepository.FindRiffleAsync(id);
            if (riffle == null)
            {
                var NotFoundResponse = new ApiResponse<List<object>>(false, 404, new List<object> { "Riffle not found" });
                return Ok(NotFoundResponse);
            }
            var successResponse = new ApiResponse<Riffle>(true, 200, riffle);
            return Ok(successResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPut("updateRiffle/{id}")]
    public async Task<IActionResult> PutAsync(int id, Riffle riffle)
    {
        try
        {
            var currentRiffle = await _riffleRepository.FindRiffleAsync(id);
            if (currentRiffle == null)
            {
                var NotFoundResponse = new ApiResponse<List<object>>(false, 404, new List<object> { "Riffle not found" });
                return Ok(NotFoundResponse);
            }
            currentRiffle.Name = riffle.Name;
            currentRiffle.Description = riffle.Description;
            currentRiffle.InitDate = riffle.InitDate;
            currentRiffle.EndtDate = riffle.EndtDate;
            await _riffleRepository.UpdateAsync(currentRiffle);
            var successResponse = new ApiResponse<List<Riffle>>(true, 200, new List<Riffle> { currentRiffle });
            return Ok(successResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpDelete("deleteRiffle/{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            var riffle = await _riffleRepository.FindRiffleAsync(id);
            if (riffle == null)
            {
                var NotFoundResponse = new ApiResponse<List<object>>(false, 404, new List<object> { "Riffle not found" });
                return Ok(NotFoundResponse);
            }
            foreach (var ticket in riffle.Tickets.ToList())
            {
                ticket.RiffleId = null;
            }
            await _riffleRepository.DeleteAsync(riffle);
            var successResponse = new ApiResponse<List<Riffle>>(true, 200, []);
            return Ok(successResponse);
        }
        catch (Exception ex)
        {
            var sqlException = new ApiResponse<List<object>>(false, 404, new List<object> { "Unexpected error deleting record..." });
            return Conflict(sqlException);
        }
    }
}