using Coney.Backend.Data;
using Coney.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coney.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly DataContext _context;

    public CommentsController(DataContext context)
    {
        _context = context;
    }

    [HttpPost("createComment")]
    public async Task<IActionResult> PostAsync(Comment comment)
    {
        _context.Add(comment);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<Comment>(true, 200, comment);
        return Ok(successResponse);
    }

    [HttpGet("getAllComments")]
    public async Task<IActionResult> GetAsync()
    {
        var successResponse = new ApiResponse<List<Comment>>(true, 200, await _context.Comments.ToListAsync());
        return Ok(successResponse);
    }

    [HttpGet("getComment/{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        var successResponse = new ApiResponse<Comment>(true, 200, comment);
        return Ok(successResponse);
    }

    [HttpPut("updateComment")]
    public async Task<IActionResult> PutAsync(Comment comment)
    {
        var currentComment = await _context.Comments.FindAsync(comment.Id);
        if (currentComment == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        currentComment.Observations = comment.Observations;
        currentComment.UserId = comment.UserId;
        currentComment.RiffleId = comment.RiffleId;
        _context.Update(currentComment);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<List<Comment>>(true, 200, []);
        return Ok(successResponse);
    }

    [HttpDelete("deleteComment/{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            var NotFoundResponse = new ApiResponse<List<object>>(false, 404, []);
            return Ok(NotFoundResponse);
        }
        _context.Remove(comment);
        await _context.SaveChangesAsync();
        var successResponse = new ApiResponse<List<Comment>>(true, 200, []);
        return Ok(successResponse);
    }
}