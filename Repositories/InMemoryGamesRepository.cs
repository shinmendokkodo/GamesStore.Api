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

    public IEnumerable<Game> GetAll()
    {
        return games;
    }

    public Game? GetById(int id)
    {
        return games.Find(g => g.Id == id);
    }

    public void Create(Game game)
    {
        game.Id = games.Max(g => g.Id) + 1;
        games.Add(game);
    }

    public void Update(Game updatedGame)
    {
        var index = games.FindIndex(g => g.Id == updatedGame.Id);
        games[index] = updatedGame;
    }

    public void Delete(int id)
    {
        var index = games.FindIndex(g => g.Id == id);
        games.RemoveAt(index);
    }
}