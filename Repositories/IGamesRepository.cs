using GamesStore.Api.Models.Entities;

namespace GamesStore.Api.Repositories;

public interface IGamesRepository
{
    Task CreateAsync(Game game);
    Task DeleteAsync(int id);
    Task<IEnumerable<Game>> GetAllAsync(int pageNumber, int pageSize);
    Task<Game?> GetByIdAsync(int id);
    Task UpdateAsync(Game updatedGame);
    Task<int> GetCountAsync();
}
