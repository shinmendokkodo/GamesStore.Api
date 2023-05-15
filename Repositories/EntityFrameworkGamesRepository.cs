using GamesStore.Api.Data;
using GamesStore.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GamesStore.Api.Repositories;

public class EntityFrameworkGamesRepository : IGamesRepository
{
    private readonly GameStoreContext context;

    public EntityFrameworkGamesRepository(GameStoreContext context)
    {
        this.context = context;
    }

    public async Task CreateAsync(Game game)
    {
        await context.Games.AddAsync(game);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await context.Games.Where(g => g.Id == id).ExecuteDeleteAsync();
    }

    public async Task<IEnumerable<Game>> GetAllAsync()
    {
        return await context.Games.AsNoTracking().ToListAsync();
    }

    public async Task<Game?> GetByIdAsync(int id)
    {
        return await context.Games.FindAsync(id);
    }

    public async Task UpdateAsync(Game updatedGame)
    {
        context.Update(updatedGame);
        await context.SaveChangesAsync();
    }
}