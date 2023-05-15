using GamesStore.Api.Entities;

const string GetGameEndPointName = "GetGame";

List<Game> games = new List<Game>()
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

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

var group = app.MapGroup("/games");

group.MapGet("/", () => games);

group.MapGet("/{id}", (int id) =>
{
    var game = games.Find(g => g.Id == id);

    if (game == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(game);
})
.WithName(GetGameEndPointName);

group.MapPost("/", (Game game) =>
{
    game.Id = games.Max(g => g.Id) + 1;

    games.Add(game);

    return Results.CreatedAtRoute(GetGameEndPointName, new { id = game.Id }, game);
});

group.MapPut("/{id}", (int id, Game updatedGame) =>
{
    var existingGame = games.Find(g => g.Id == id);

    if (existingGame == null)
    {
        return Results.NotFound();
    }

    existingGame.Name = updatedGame.Name;
    existingGame.Genre = updatedGame.Genre;
    existingGame.Price = updatedGame.Price;
    existingGame.ReleaseDate = updatedGame.ReleaseDate;
    existingGame.ImageUri = updatedGame.ImageUri;

    return Results.NoContent();
});

group.MapDelete("/{id}", (int id) =>
{
    var game = games.Find(g => g.Id == id);

    if (game == null)
    {
        return Results.NotFound();
    }

    games.Remove(game);

    return Results.NoContent();
});

app.Run();
