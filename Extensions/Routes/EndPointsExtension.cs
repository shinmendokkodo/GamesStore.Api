using System.Diagnostics;
using GamesStore.Api.Authorization;
using GamesStore.Api.Extensions.Models;
using GamesStore.Api.Models.Dtos;
using GamesStore.Api.Repositories;

namespace GamesStore.Api.Extensions.Routes;

public static class EndPointsExtension
{
    private const string GetGameV1EndPointName = "GetGameV1";
    private const string GetGameV2EndPointName = "GetGameV2";

    public static RouteGroupBuilder MapEndPoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.NewVersionedApi()
            .MapGroup("/games")
            .HasApiVersion(1.0)
            .HasApiVersion(2.0)
            .WithParameterValidation();

        #region V1Apis

        group.MapGet("/", async (
            IGamesRepository repository,
            ILoggerFactory loggerFactory,
            [AsParameters] GetGamesDtoV1 request,
            HttpContext httpContext) =>
        {
            var totalCount = await repository.GetCountAsync();
            httpContext.Response.AddPaginationHeader(totalCount, request.PageSize);
            return Results.Ok((await repository.GetAllAsync(
                request.PageNumber, request.PageSize)).Select(g => g.AsDtoV1()));
        })
        .MapToApiVersion(1.0);

        group.MapGet("/{id}", async (IGamesRepository repository, int id) =>
        {
            var game = await repository.GetByIdAsync(id);
            return game is not null ? Results.Ok(game.AsDtoV1()) : Results.NotFound();
        })
        .MapToApiVersion(1.0)
        .WithName(GetGameV1EndPointName)
        .RequireAuthorization(Policies.ReadAccess);

        #endregion

        #region V2Apis

        group.MapGet("/", async (
            IGamesRepository repository,
            ILoggerFactory loggerFactory,
            [AsParameters] GetGamesDtoV2 request,
            HttpContext httpContext) =>
        {
            var totalCount = await repository.GetCountAsync();
            httpContext.Response.AddPaginationHeader(totalCount, request.PageSize);
            return Results.Ok((await repository.GetAllAsync(
                request.PageNumber, request.PageSize)).Select(g => g.AsDtoV2()));
        })
        .MapToApiVersion(2.0);

        group.MapGet("/{id}", async (IGamesRepository repository, int id) =>
        {
            var game = await repository.GetByIdAsync(id);
            return game is not null ? Results.Ok(game.AsDtoV2()) : Results.NotFound();
        })
        .MapToApiVersion(2.0)
        .WithName(GetGameV2EndPointName)
        .RequireAuthorization(Policies.ReadAccess);

        #endregion

        group.MapPost("/", async (IGamesRepository repository,
            CreateGameDto CreateGameDto) =>
        {
            var game = CreateGameDto.AsEntity();
            await repository.CreateAsync(game);
            return Results.CreatedAtRoute(GetGameV1EndPointName, new { id = game.Id }, game);
        })
        .MapToApiVersion(1.0)
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
        .MapToApiVersion(1.0)
        .RequireAuthorization(Policies.WriteAccess);

        group.MapDelete("/{id}", async (IGamesRepository repository, int id) =>
        {
            var game = await repository.GetByIdAsync(id);

            if (game is null) return Results.NotFound();

            await repository.DeleteAsync(id);
            return Results.NoContent();
        })
        .MapToApiVersion(1.0)
        .RequireAuthorization(Policies.WriteAccess);

        return group;
    }
}
