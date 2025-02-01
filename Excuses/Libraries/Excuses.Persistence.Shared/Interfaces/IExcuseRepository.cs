using Excuses.Persistence.Shared.DTO;
using Excuses.Persistence.Shared.Models;
using Excuses.Persistence.Shared.Utils;

namespace Excuses.Persistence.Shared.Interfaces;

public interface IExcuseRepository
{
    Task<Result<Excuse>> CreateExcuseAsync(ExcuseInputDto excuse);
    Task<Result<List<Excuse>>> GetExcusesAsync();
    Task<Result<List<Excuse>>> GetExcusesByCategoryAsync(string category);
    Task<Result<Excuse>> GetExcuseAsync(int id);
    Task<Result<Excuse>> GetRandomExcuseAsync();
    Task<Result<Excuse>> GetRandomExcuseByCategoryAsync(string category);
    Task<Result<List<string>>> GetCategoriesAsync();
    Task<Result<Excuse>> UpdateExcuseAsync(int id, ExcuseInputDto excuse);
    Task<Result<Excuse>> DeleteExcuseAsync(int id);
}