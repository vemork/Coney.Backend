using Coney.Backend.Data;
using Coney.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coney.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly DataContext _context;

    public NotificationsController(DataContext context)
    {
        _context = context;
    }

    [HttpPost("createNotification")]
    public async Task<IActionResult> PostAsync(Notification notification)
    {
        _context.Add(notification);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<Notification>(true, 200, notification);
        return Ok(successResponse);
    }

    [HttpGet("getAllNotifications")]
    public async Task<IActionResult> GetAsync()
    {
        var successResponse = new ApiResponse<List<Notification>>(true, 200, await _context.Notifications.ToListAsync());
        return Ok(successResponse);
    }

    [HttpGet("getNotification/{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var notification = await _context.Notifications.FindAsync(id);
        if (notification == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        var successResponse = new ApiResponse<Notification>(true, 200, notification);
        return Ok(successResponse);
    }

    [HttpPut("updateNotification")]
    public async Task<IActionResult> PutAsync(Notification notification)
    {
        var currentNotification = await _context.Notifications.FindAsync(notification.Id);
        if (currentNotification == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }

        currentNotification.Title = notification.Title;
        currentNotification.Description = notification.Description;

        _context.Update(currentNotification);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<Notification>(true, 200, currentNotification);
        return Ok(successResponse);
    }

    [HttpDelete("deleteNotification/{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var notification = await _context.Notifications.FindAsync(id);
        if (notification == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        _context.Remove(notification);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<List<Notification>>(true, 200, []);
        return Ok(successResponse);
    }
}