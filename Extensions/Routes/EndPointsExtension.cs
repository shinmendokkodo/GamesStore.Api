using System.Diagnostics;
using GamesStore.Api.Authorization;
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

        group.MapGet("/", async (IGamesRepository repository, ILoggerFactory loggerFactory) =>
        {
            return Results.Ok((await repository.GetAllAsync()).Select(g => g.AsDto()));
        });

        group.MapGet("/{id}", async (IGamesRepository repository, int id) =>
        {
            var game = await repository.GetByIdAsync(id);
            return game is not null ? Results.Ok(game.AsDto()) : Results.NotFound();
        })
        .WithName(GetGameEndPointName)
        .RequireAuthorization(Policies.ReadAccess);

        group.MapPost("/", async (IGamesRepository repository, CreateGameDto CreateGameDto) =>
        {
            var game = CreateGameDto.AsEntity();
            await repository.CreateAsync(game);
            return Results.CreatedAtRoute(GetGameEndPointName, new { id = game.Id }, game);
        })
        .RequireAuthorization(Policies.WriteAccess);

        group.MapPut("/{id}", async (IGamesRepository repository, int id, UpdateGameDto updateGameDto) =>
        {
            var existingGame = await repository.GetByIdAsync(id);

            if (existingGame is null) return Results.NotFound();

            existingGame.Name = updateGameDto.Name;
            existingGame.Genre = updateGameDto.Genre;
            existingGame.Price = updateGameDto.Price;
            existingGame.ReleaseDate = updateGameDto.ReleaseDate;
            existingGame.ImageUri = updateGameDto.ImageUri;

            await repository.UpdateAsync(existingGame);
            return Results.NoContent();
        })
        .RequireAuthorization(Policies.WriteAccess);

        group.MapDelete("/{id}", async (IGamesRepository repository, int id) =>
        {
            var game = await repository.GetByIdAsync(id);

            if (game is null) return Results.NotFound();

            await repository.DeleteAsync(id);
            return Results.NoContent();
        })
        .RequireAuthorization(Policies.WriteAccess);

        return group;
    }
}
