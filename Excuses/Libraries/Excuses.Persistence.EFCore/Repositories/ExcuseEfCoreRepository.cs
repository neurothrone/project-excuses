using Excuses.Persistence.EFCore.Data;
using Excuses.Persistence.Shared.DTO;
using Excuses.Persistence.Shared.Interfaces;
using Excuses.Persistence.Shared.Models;
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

    public async Task<Excuse?> CreateExcuseAsync(ExcuseInputDto excuse)
    {
        try
        {
            var newExcuse = new Excuse { Text = excuse.Text, Category = excuse.Category };
            await _context.Excuses.AddAsync(newExcuse);
            await _context.SaveChangesAsync();
            return newExcuse;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError($"❌ -> Failed to create Excuse: {ex.Message}");
            return null;
        }
    }

    public async Task<List<Excuse>> GetExcusesAsync()
    {
        return await _context.Excuses
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Excuse>> GetExcusesByCategoryAsync(string category)
    {
        return await _context.Excuses
            .AsNoTracking()
            .Where(e => e.Category == category)
            .ToListAsync();
    }

    public async Task<Excuse?> GetExcuseAsync(int id)
    {
        return await _context.Excuses
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Excuse?> GetRandomExcuseAsync()
    {
        return await _context.Excuses
            .AsNoTracking()
            .OrderBy(e => EF.Functions.Random())
            .FirstOrDefaultAsync();
    }

    public async Task<Excuse?> GetRandomExcuseByCategoryAsync(string category)
    {
        return await _context.Excuses
            .AsNoTracking()
            .Where(e => e.Category == category)
            .OrderBy(e => EF.Functions.Random())
            .FirstOrDefaultAsync();
    }

    public async Task<List<string>> GetCategoriesAsync()
    {
        return await _context.Excuses
            .AsNoTracking()
            .Select(e => e.Category)
            .Distinct()
            .ToListAsync();
    }

    public async Task<bool> UpdateExcuseAsync(int id, ExcuseInputDto excuse)
    {
        try
        {
            var existingExcuse = await _context.Excuses.FindAsync(id);
            if (existingExcuse is null)
                return false;

            existingExcuse.Text = excuse.Text;
            existingExcuse.Category = excuse.Category;
            await _context.SaveChangesAsync();

            return true;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError($"❌ -> Failed to update Excuse, error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteExcuseAsync(int id)
    {
        try
        {
            var excuse = await _context.Excuses.FindAsync(id);
            if (excuse is null)
                return false;

            _context.Excuses.Remove(excuse);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError($"❌ -> Failed to delete Excuse, error: {ex.Message}");
            return false;
        }
    }
}