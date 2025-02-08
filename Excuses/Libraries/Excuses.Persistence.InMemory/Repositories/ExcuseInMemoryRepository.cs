using Excuses.Persistence.InMemory.Data;
using Excuses.Persistence.Shared.DTO;
using Excuses.Persistence.Shared.Interfaces;
using Excuses.Persistence.Shared.Models;
using Excuses.Persistence.Shared.Utils;

namespace Excuses.Persistence.InMemory.Repositories;

public class ExcuseInMemoryRepository : IExcuseRepository
{
    private readonly InMemoryDataStore _dataStore;

    private int _currentExcuseId;

    public ExcuseInMemoryRepository(InMemoryDataStore dataStore)
    {
        _dataStore = dataStore;
        _currentExcuseId = _dataStore.Excuses.Max(e => e.Id);
    }

    public async Task<Result<Excuse>> CreateExcuseAsync(ExcuseInputDto excuse)
    {
        await Task.Delay(TimeSpan.FromSeconds(1));

        var newExcuse = new Excuse
        {
            Id = ++_currentExcuseId,
            Text = excuse.Text,
            Category = excuse.Category
        };
        _dataStore.Excuses.Add(newExcuse);

        return Result<Excuse>.Success(newExcuse);
    }

    public async Task<Result<List<Excuse>>> GetExcusesAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(2));
        return Result<List<Excuse>>.Success(_dataStore.Excuses);
    }

    public async Task<Result<List<Excuse>>> GetExcusesByCategoryAsync(string category)
    {
        await Task.Delay(TimeSpan.FromSeconds(1));

        var excuses = _dataStore.Excuses.Where(e => e.Category == category).ToList();
        return Result<List<Excuse>>.Success(excuses);
    }

    public async Task<Result<Excuse>> GetRandomExcuseAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(1));

        if (_dataStore.Excuses.Count == 0)
            return Result<Excuse>.Failure(ExcuseMessages.NoExcuses);

        var excuse = _dataStore.Excuses[new Random().Next(_dataStore.Excuses.Count)];
        return Result<Excuse>.Success(excuse);
    }

    public async Task<Result<Excuse>> GetRandomExcuseByCategoryAsync(string category)
    {
        await Task.Delay(TimeSpan.FromSeconds(1));

        var excuses = _dataStore.Excuses
            .Where(e => e.Category == category)
            .ToList();

        if (excuses.Count == 0)
            return Result<Excuse>.Failure(ExcuseMessages.NoExcusesForCategory(category));

        var excuse = excuses[new Random().Next(excuses.Count)];
        return Result<Excuse>.Success(excuse);
    }

    public async Task<Result<Excuse>> GetExcuseAsync(int id)
    {
        await Task.Delay(TimeSpan.FromSeconds(1));

        var excuse = _dataStore.Excuses.FirstOrDefault(e => e.Id == id);
        return excuse is null
            ? Result<Excuse>.Failure(ExcuseMessages.ExcuseNotFound)
            : Result<Excuse>.Success(excuse);
    }

    public async Task<Result<List<string>>> GetCategoriesAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(1));

        var categories = _dataStore.Excuses
            .Select(e => e.Category)
            .Distinct()
            .ToList();
        return Result<List<string>>.Success(categories);
    }

    public async Task<Result<Excuse>> UpdateExcuseAsync(int id, ExcuseInputDto excuse)
    {
        await Task.Delay(TimeSpan.FromSeconds(1));

        var excuseToUpdate = _dataStore.Excuses.FirstOrDefault(e => e.Id == id);
        if (excuseToUpdate is null)
            return Result<Excuse>.Failure(ExcuseMessages.ExcuseNotFound);

        excuseToUpdate.Text = excuse.Text;
        excuseToUpdate.Category = excuse.Category;
        return Result<Excuse>.Success(excuseToUpdate);
    }

    public async Task<Result<Excuse>> DeleteExcuseAsync(int id)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(500));

        var excuseToDelete = _dataStore.Excuses.FirstOrDefault(e => e.Id == id);
        if (excuseToDelete is null)
            return Result<Excuse>.Failure(ExcuseMessages.ExcuseNotFound);

        _dataStore.Excuses.Remove(excuseToDelete);
        return Result<Excuse>.Success(excuseToDelete);
    }
}