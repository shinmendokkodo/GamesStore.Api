using GamesStore.Api.Extensions.Models;
using GamesStore.Api.Models.Dtos;
using GamesStore.Api.Repositories;

namespace GamesStore.Api.Extensions.Routes;

public static class EndPointsExtension
{
    const string GetGameEndPointName = "GetGame";

    public static RouteGroupBuilder MapEndPoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/games").WithParameterValidation();

        group.MapGet("/", (IGamesRepository repository) => repository.GetAll().Select(g => g.AsDto()));

        group.MapGet("/{id}", (IGamesRepository repository, int id) =>
        {
            var game = repository.GetById(id);
            return game is not null ? Results.Ok(game.AsDto()) : Results.NotFound();
        })
        .WithName(GetGameEndPointName);

        group.MapPost("/", (IGamesRepository repository, CreateGameDto CreateGameDto) =>
        {
            var game = CreateGameDto.AsEntity();
            repository.Create(game);
            return Results.CreatedAtRoute(GetGameEndPointName, new { id = game.Id }, game);
        });

        group.MapPut("/{id}", (IGamesRepository repository, int id, UpdateGameDto updateGameDto) =>
        {
            var existingGame = repository.GetById(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            existingGame.Name = updateGameDto.Name;
            existingGame.Genre = updateGameDto.Genre;
            existingGame.Price = updateGameDto.Price;
            existingGame.ReleaseDate = updateGameDto.ReleaseDate;
            existingGame.ImageUri = updateGameDto.ImageUri;

            repository.Update(existingGame);

            return Results.NoContent();
        });

        group.MapDelete("/{id}", (IGamesRepository repository, int id) =>
        {
            var game = repository.GetById(id);

            if (game is null)
            {
                return Results.NotFound();
            }

            repository.Delete(id);

            return Results.NoContent();
        });

        return group;
    }
}
