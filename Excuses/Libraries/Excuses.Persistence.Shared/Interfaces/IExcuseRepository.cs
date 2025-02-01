using Excuses.Persistence.Shared.DTO;
using Excuses.Persistence.Shared.Models;

namespace Excuses.Persistence.Shared.Interfaces;

public interface IExcuseRepository
{
    Task<Excuse?> CreateExcuseAsync(ExcuseInputDto excuse);
    Task<List<Excuse>> GetExcusesAsync();
    Task<List<Excuse>> GetExcusesByCategoryAsync(string category);
    Task<Excuse?> GetExcuseAsync(int id);
    Task<Excuse?> GetRandomExcuseAsync();
    Task<Excuse?> GetRandomExcuseByCategoryAsync(string category);
    Task<List<string>> GetCategoriesAsync();
    Task<bool> UpdateExcuseAsync(int id, ExcuseInputDto excuse);
    Task<bool> DeleteExcuseAsync(int id);
}