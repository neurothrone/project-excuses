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

    public Task<Result<Excuse>> CreateExcuseAsync(ExcuseInputDto excuse)
    {
        var newExcuse = new Excuse
        {
            Id = ++_currentExcuseId,
            Text = excuse.Text,
            Category = excuse.Category
        };
        _dataStore.Excuses.Add(newExcuse);
        return Task.FromResult(Result<Excuse>.Success(newExcuse));
    }

    public Task<Result<List<Excuse>>> GetExcusesAsync()
    {
        return Task.FromResult(Result<List<Excuse>>.Success(_dataStore.Excuses));
    }

    public Task<Result<List<Excuse>>> GetExcusesByCategoryAsync(string category)
    {
        var excuses = _dataStore.Excuses.Where(e => e.Category == category).ToList();
        return Task.FromResult(Result<List<Excuse>>.Success(excuses));
    }

    public Task<Result<Excuse>> GetRandomExcuseAsync()
    {
        if (_dataStore.Excuses.Count == 0)
            return Task.FromResult(Result<Excuse>.Failure(ExcuseMessages.NoExcuses));

        var excuse = _dataStore.Excuses[new Random().Next(_dataStore.Excuses.Count)];
        return Task.FromResult(Result<Excuse>.Success(excuse));
    }

    public Task<Result<Excuse>> GetRandomExcuseByCategoryAsync(string category)
    {
        var excuses = _dataStore.Excuses
            .Where(e => e.Category == category)
            .ToList();

        if (excuses.Count == 0)
            return Task.FromResult(Result<Excuse>.Failure(ExcuseMessages.NoExcusesForCategory(category)));

        var excuse = excuses[new Random().Next(excuses.Count)];
        return Task.FromResult(Result<Excuse>.Success(excuse));
    }

    public Task<Result<Excuse>> GetExcuseAsync(int id)
    {
        var excuse = _dataStore.Excuses.FirstOrDefault(e => e.Id == id);
        return Task.FromResult(excuse is null
            ? Result<Excuse>.Failure(ExcuseMessages.ExcuseNotFound)
            : Result<Excuse>.Success(excuse));
    }

    public Task<Result<List<string>>> GetCategoriesAsync()
    {
        var categories = _dataStore.Excuses
            .Select(e => e.Category)
            .Distinct()
            .ToList();
        return Task.FromResult(Result<List<string>>.Success(categories));
    }

    public Task<Result<Excuse>> UpdateExcuseAsync(int id, ExcuseInputDto excuse)
    {
        var excuseToUpdate = _dataStore.Excuses.FirstOrDefault(e => e.Id == id);
        if (excuseToUpdate is null)
            return Task.FromResult(Result<Excuse>.Failure(ExcuseMessages.ExcuseNotFound));

        excuseToUpdate.Text = excuse.Text;
        excuseToUpdate.Category = excuse.Category;
        return Task.FromResult(Result<Excuse>.Success(excuseToUpdate));
    }

    public Task<Result<Excuse>> DeleteExcuseAsync(int id)
    {
        var excuseToDelete = _dataStore.Excuses.FirstOrDefault(e => e.Id == id);
        if (excuseToDelete is null)
            return Task.FromResult(Result<Excuse>.Failure(ExcuseMessages.ExcuseNotFound));

        _dataStore.Excuses.Remove(excuseToDelete);
        return Task.FromResult(Result<Excuse>.Success(excuseToDelete));
    }
}