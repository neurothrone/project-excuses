using Excuses.Persistence.EFCore.Data;
using Excuses.Persistence.Shared.DTO;
using Excuses.Persistence.Shared.Interfaces;
using Excuses.Persistence.Shared.Models;
using Excuses.Persistence.Shared.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Excuses.Persistence.EFCore.Repositories;

public class ExcuseEfCoreRepository : IExcuseRepository
{
    private readonly ApiDbContext _context;
    private readonly ILogger<ExcuseEfCoreRepository> _logger;

    public ExcuseEfCoreRepository(
        ApiDbContext context,
        ILogger<ExcuseEfCoreRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Excuse>> CreateExcuseAsync(ExcuseInputDto excuse)
    {
        try
        {
            var newExcuse = new Excuse { Text = excuse.Text, Category = excuse.Category };
            await _context.Excuses.AddAsync(newExcuse);
            await _context.SaveChangesAsync();
            return Result<Excuse>.Success(newExcuse);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError($"❌ -> Failed to create Excuse: {ex.Message}");
            return Result<Excuse>.Failure(ExcuseMessages.DbUpdateFailed);
        }
    }

    public async Task<Result<List<Excuse>>> GetExcusesAsync()
    {
        try
        {
            var excuses = await _context.Excuses
                .AsNoTracking()
                .ToListAsync();
            return Result<List<Excuse>>.Success(excuses);
        }
        catch (Exception ex)
        {
            _logger.LogError($"❌ -> Failed to get Excuses: {ex.Message}");
            return Result<List<Excuse>>.Failure(ExcuseMessages.DbReadFailed);
        }
    }

    public async Task<Result<List<Excuse>>> GetExcusesByCategoryAsync(string category)
    {
        try
        {
            var excuses = await _context.Excuses
                .AsNoTracking()
                .Where(e => e.Category == category)
                .ToListAsync();
            return Result<List<Excuse>>.Success(excuses);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError($"❌ -> Failed to get Excuses by category: {ex.Message}");
            return Result<List<Excuse>>.Failure(ExcuseMessages.DbReadFailed);
        }
    }

    public async Task<Result<Excuse>> GetExcuseAsync(int id)
    {
        try
        {
            var excuse = await _context.Excuses
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
            return excuse is not null
                ? Result<Excuse>.Success(excuse)
                : Result<Excuse>.Failure(ExcuseMessages.ExcuseNotFound);
        }
        catch (Exception ex)
        {
            _logger.LogError($"❌ -> Failed to get Excuse: {ex.Message}");
            return Result<Excuse>.Failure(ExcuseMessages.DbReadFailed);
        }
    }

    public async Task<Result<Excuse>> GetRandomExcuseAsync()
    {
        try
        {
            var excuse = await _context.Excuses
                .AsNoTracking()
                .OrderBy(_ => Guid.NewGuid())
                .Take(1)
                .FirstOrDefaultAsync();
            return excuse is not null
                ? Result<Excuse>.Success(excuse)
                : Result<Excuse>.Failure(ExcuseMessages.NoExcuses);
        }
        catch (Exception ex)
        {
            _logger.LogError($"❌ -> Failed to get random Excuse: {ex.Message}");
            return Result<Excuse>.Failure(ExcuseMessages.DbReadFailed);
        }
    }

    public async Task<Result<Excuse>> GetRandomExcuseByCategoryAsync(string category)
    {
        try
        {
            var excuse = await _context.Excuses
                .AsNoTracking()
                .Where(e => e.Category == category)
                .OrderBy(_ => Guid.NewGuid())
                .Take(1)
                .FirstOrDefaultAsync();
            return excuse is not null
                ? Result<Excuse>.Success(excuse)
                : Result<Excuse>.Failure(ExcuseMessages.NoExcusesForCategory(category));
        }
        catch (Exception ex)
        {
            _logger.LogError($"❌ -> Failed to get random Excuse by category: {ex.Message}");
            return Result<Excuse>.Failure(ExcuseMessages.DbReadFailed);
        }
    }

    public async Task<Result<List<string>>> GetCategoriesAsync()
    {
        try
        {
            var categories = await _context.Excuses
                .AsNoTracking()
                .Select(e => e.Category)
                .Distinct()
                .ToListAsync();
            return Result<List<string>>.Success(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError($"❌ -> Failed to get Excuse categories: {ex.Message}");
            return Result<List<string>>.Failure(ExcuseMessages.DbReadFailed);
        }
    }

    public async Task<Result<Excuse>> UpdateExcuseAsync(int id, ExcuseInputDto excuse)
    {
        try
        {
            var existingExcuse = await _context.Excuses.FindAsync(id);
            if (existingExcuse is null)
                return Result<Excuse>.Failure(ExcuseMessages.ExcuseNotFound);

            existingExcuse.Text = excuse.Text;
            existingExcuse.Category = excuse.Category;
            await _context.SaveChangesAsync();

            return Result<Excuse>.Success(existingExcuse);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError($"❌ -> Failed to update Excuse, error: {ex.Message}");
            return Result<Excuse>.Failure(ExcuseMessages.DbUpdateFailed);
        }
    }

    public async Task<Result<Excuse>> DeleteExcuseAsync(int id)
    {
        try
        {
            var excuse = await _context.Excuses.FindAsync(id);
            if (excuse is null)
                return Result<Excuse>.Failure(ExcuseMessages.ExcuseNotFound);

            _context.Excuses.Remove(excuse);
            await _context.SaveChangesAsync();
            return Result<Excuse>.Success(excuse);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError($"❌ -> Failed to delete Excuse, error: {ex.Message}");
            return Result<Excuse>.Failure(ExcuseMessages.DbUpdateFailed);
        }
    }
}