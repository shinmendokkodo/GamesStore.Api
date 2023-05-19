using GamesStore.Api.Data;
using GamesStore.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GamesStore.Api.Repositories;

public class EntityFrameworkGamesRepository : IGamesRepository
{
    private readonly GameStoreContext context;
    private readonly ILogger<EntityFrameworkGamesRepository> logger;

    public EntityFrameworkGamesRepository(GameStoreContext context, ILogger<EntityFrameworkGamesRepository> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task<IEnumerable<Game>> GetAllAsync(int pageNumber, int pageSize, string? filter)
    {
        int skipCount = (pageNumber - 1) * pageSize;
        return await FilterGames(filter)
                    .OrderBy(g => g.Id)
                    .Skip(skipCount)
                    .Take(pageSize)
                    .AsNoTracking()
                    .ToListAsync();
    }

    public async Task<Game?> GetByIdAsync(int id)
    {
        return await context.Games.FindAsync(id);
    }

    public async Task CreateAsync(Game game)
    {
        await context.Games.AddAsync(game);
        await context.SaveChangesAsync();

        logger.LogInformation("Created game {Name} with id {Id} and price {Price}", game.Name, game.Id, game.Price);
    }

    public async Task UpdateAsync(Game updatedGame)
    {
        context.Update(updatedGame);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await context.Games.Where(g => g.Id == id).ExecuteDeleteAsync();
    }

    public async Task<int> GetCountAsync(string? filter)
    {
        return await FilterGames(filter).CountAsync();
    }

    private IQueryable<Game> FilterGames(string? filter)
    {
        return string.IsNullOrWhiteSpace(filter)
            ? context.Games
            : context.Games.Where(g => g.Name.Contains(filter)
                || g.Genre.Contains(filter));
    }
}
