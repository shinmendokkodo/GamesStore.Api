using GamesStore.Api.Models.Entities;

namespace GamesStore.Api.Repositories;

public class InMemoryGamesRepository : IGamesRepository
{
    private readonly List<Game> games = new List<Game>()
    {
        new Game()
        {
            Id = 1,
            Name = "The Elder Scrolls V: Skyrim",
            Genre = "Role-playing",
            Price = 39.99m,
            ReleaseDate = new DateTime(2011, 11, 11),
            ImageUri = "https://placeholder.co/100"
        },
        new Game()
        {
            Id = 2,
            Name = "Minecraft",
            Genre = "Sandbox, Survival",
            Price = 26.95m,
            ReleaseDate = new DateTime(2011, 11, 18),
            ImageUri = "https://placeholder.co/100"
        },
        new Game
        {
            Id = 3,
            Name = "The Witcher 3: Wild Hunt",
            Genre = "Action role-playing",
            Price = 49.99m,
            ReleaseDate = new DateTime(2015, 5, 19),
            ImageUri = "https://placeholder.co/100"
        }
    };

    public async Task<Game?> GetByIdAsync(int id)
    {
        return await Task.FromResult(games.Find(g => g.Id == id));
    }

    public async Task CreateAsync(Game game)
    {
        game.Id = games.Max(g => g.Id) + 1;
        games.Add(game);

        await Task.CompletedTask;
    }

    public async Task UpdateAsync(Game updatedGame)
    {
        var index = games.FindIndex(g => g.Id == updatedGame.Id);
        games[index] = updatedGame;

        await Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var index = games.FindIndex(g => g.Id == id);
        games.RemoveAt(index);

        await Task.CompletedTask;
    }

    public async Task<IEnumerable<Game>> GetAllAsync(int pageNumber, int pageSize, string? filter)
    {
        int skipCount = (pageNumber - 1) * pageSize;
        return await Task.FromResult(FilterGames(filter).Skip(skipCount).Take(pageSize));
    }

    public async Task<int> GetCountAsync(string? filter)
    {
        return await Task.FromResult(FilterGames(filter).Count());
    }

    private IEnumerable<Game> FilterGames(string? filter)
    {
        return string.IsNullOrWhiteSpace(filter)
            ? games
            : games.Where(g => g.Name.Contains(filter)
                || g.Genre.Contains(filter));
    }
}