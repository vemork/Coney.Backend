using Coney.Backend.Data;
using Coney.Backend.Repositories.interfaces;
using Coney.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace Coney.Backend.Repositories.implementations;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly DataContext _context;
    private readonly DbSet<T> _entity;

    public GenericRepository(DataContext context)
    {
        _context = context;
        _entity = context.Set<T>();
    }

    public virtual async Task<ActionResponse<T>> AddAsync(T entity)
    {
        _context.Add(entity);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<T>
            {
                Status = true,
                Code = 200,
                Data = entity,
                Message = "POSTOK001"
            };
        }
        catch (DbUpdateException)
        {
            return DbUpdateExceptionActionResponse();
        }
        catch (Exception ex)
        {
            return ExceptionActionResponse(ex);
        }
    }

    public virtual async Task<ActionResponse<T>> DeleteAsync(int id)
    {
        var row = await _entity.FindAsync(id);
        if (row == null)
        {
            return new ActionResponse<T>
            {
                Status = false,
                Code = 404,
                Data = null,
                Message = "DELFAIL001"
            };
        }

        _entity.Remove(row);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<T>
            {
                Status = true,
                Code = 200,
                Data = null,
                Message = "DELOK002"
            };
        }
        catch
        {
            return new ActionResponse<T>
            {
                Status = false,
                Code = 500,
                Data = null,
                Message = "DELFAIL003"
            };
        }
    }

    public virtual async Task<ActionResponse<T>> GetAsync(int id)
    {
        var row = await _entity.FindAsync(id);
        if (row == null)
        {
            return new ActionResponse<T>
            {
                Status = false,
                Code = 404,
                Data = null,
                Message = "GETFAIL001"
            };
        }
        return new ActionResponse<T>
        {
            Status = true,
            Code = 200,
            Data = row,
            Message = "GETOK001"
        };
    }

    public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync()
    {
        return new ActionResponse<IEnumerable<T>>
        {
            Status = true,
            Code = 200,
            Data = await _entity.ToListAsync(),
            Message = "GETALL001"
        };
    }

    public virtual async Task<ActionResponse<T>> UpdateAsync(T entity)
    {
        _context.Update(entity);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<T>
            {
                Status = true,
                Code = 200,
                Data = entity,
                Message = "PUTOK001"
            };
        }
        catch (DbUpdateException)
        {
            return DbUpdateExceptionActionResponse();
        }
        catch (Exception exception)
        {
            return ExceptionActionResponse(exception);
        }
    }

    private ActionResponse<T> ExceptionActionResponse(Exception ex)
    {
        return new ActionResponse<T>
        {
            Status = false,
            Code = 500,
            Data = null,
            Message = "ERR001" // Internal Server Error.
        };
    }

    private ActionResponse<T> DbUpdateExceptionActionResponse()
    {
        return new ActionResponse<T>
        {
            Status = false,
            Code = 400,
            Data = null,
            Message = "PUTFAIL001" // The record you are trying to create already exists.
        };
    }
}