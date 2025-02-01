using Excuses.Persistence.InMemory.Data;
using Excuses.Persistence.Shared.DTO;
using Excuses.Persistence.Shared.Interfaces;
using Excuses.Persistence.Shared.Models;

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

    public Task<Excuse?> CreateExcuseAsync(ExcuseInputDto excuse)
    {
        var newExcuse = new Excuse
        {
            Id = ++_currentExcuseId,
            Text = excuse.Text,
            Category = excuse.Category
        };
        _dataStore.Excuses.Add(newExcuse);
        return Task.FromResult<Excuse?>(newExcuse);
    }

    public Task<List<Excuse>> GetExcusesAsync()
    {
        return Task.FromResult(_dataStore.Excuses);
    }

    public Task<List<Excuse>> GetExcusesByCategoryAsync(string category)
    {
        var excuses = _dataStore.Excuses.Where(e => e.Category == category).ToList();
        return Task.FromResult(excuses);
    }

    public Task<Excuse?> GetRandomExcuseAsync()
    {
        if (_dataStore.Excuses.Count == 0)
            return Task.FromResult<Excuse?>(null);

        var excuse = _dataStore.Excuses[new Random().Next(_dataStore.Excuses.Count)];
        return Task.FromResult<Excuse?>(excuse);
    }

    public Task<Excuse?> GetRandomExcuseByCategoryAsync(string category)
    {
        var excuses = _dataStore.Excuses
            .Where(e => e.Category == category)
            .ToList();
        return excuses.Count == 0
            ? Task.FromResult<Excuse?>(null)
            : Task.FromResult<Excuse?>(excuses[new Random().Next(excuses.Count)]);
    }

    public Task<Excuse?> GetExcuseAsync(int id)
    {
        var excuse = _dataStore.Excuses.FirstOrDefault(e => e.Id == id);
        return Task.FromResult(excuse);
    }

    public Task<List<string>> GetCategoriesAsync()
    {
        return Task.FromResult(_dataStore.Excuses
            .Select(e => e.Category)
            .Distinct()
            .ToList());
    }

    public Task<bool> UpdateExcuseAsync(int id, ExcuseInputDto excuse)
    {
        var excuseToUpdate = _dataStore.Excuses.FirstOrDefault(e => e.Id == id);
        if (excuseToUpdate is null)
            return Task.FromResult(false);

        excuseToUpdate.Text = excuse.Text;
        excuseToUpdate.Category = excuse.Category;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteExcuseAsync(int id)
    {
        var excuseToDelete = _dataStore.Excuses.FirstOrDefault(e => e.Id == id);
        if (excuseToDelete is null)
            return Task.FromResult(false);

        _dataStore.Excuses.Remove(excuseToDelete);
        return Task.FromResult(true);
    }
}