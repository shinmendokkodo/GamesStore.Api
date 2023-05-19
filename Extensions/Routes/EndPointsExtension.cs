using GamesStore.Api.Authorization;
using GamesStore.Api.Extensions.Models;
using GamesStore.Api.Models.Dtos;
using GamesStore.Api.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

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
            .WithParameterValidation()
            .WithOpenApi()
            .WithTags("Games");

        #region V1Apis

        group.MapGet("/", async (
            IGamesRepository repository,
            ILoggerFactory loggerFactory,
            [AsParameters] GetGamesDtoV1 request,
            HttpContext httpContext) =>
        {
            var totalCount = await repository.GetCountAsync(request.Filter);
            httpContext.Response.AddPaginationHeader(totalCount, request.PageSize);
            return Results.Ok((await repository.GetAllAsync(
                request.PageNumber, request.PageSize, request.Filter)).Select(g => g.AsDtoV1()));
        })
        .MapToApiVersion(1.0)
        .WithSummary("Get all games")
        .WithDescription("Gets all available games and allows filtering and pagination");

        group.MapGet("/{id}", async Task<Results<Ok<GameDtoV1>, NotFound>> (IGamesRepository repository, int id) =>
        {
            var game = await repository.GetByIdAsync(id);
            return game is not null ? TypedResults.Ok(game.AsDtoV1()) : TypedResults.NotFound();
        })
        .MapToApiVersion(1.0)
        .WithName(GetGameV1EndPointName)
        .RequireAuthorization(Policies.ReadAccess)
        .WithSummary("Get a game by id")
        .WithDescription("Gets a game by id and returns it");

        #endregion

        #region V2Apis

        group.MapGet("/", async (
            IGamesRepository repository,
            ILoggerFactory loggerFactory,
            [AsParameters] GetGamesDtoV2 request,
            HttpContext httpContext) =>
        {
            var totalCount = await repository.GetCountAsync(request.Filter);
            httpContext.Response.AddPaginationHeader(totalCount, request.PageSize);
            return Results.Ok((await repository.GetAllAsync(
                request.PageNumber, request.PageSize, request.Filter)).Select(g => g.AsDtoV2()));
        })
        .MapToApiVersion(2.0)
        .WithSummary("Get all games")
        .WithDescription("Gets all available games and allows filtering and pagination"); ;

        group.MapGet("/{id}", async Task<Results<Ok<GameDtoV2>, NotFound>> (IGamesRepository repository, int id) =>
        {
            var game = await repository.GetByIdAsync(id);
            return game is not null ? TypedResults.Ok(game.AsDtoV2()) : TypedResults.NotFound();
        })
        .MapToApiVersion(2.0)
        .WithName(GetGameV2EndPointName)
        .RequireAuthorization(Policies.ReadAccess)
        .WithSummary("Get a game by id")
        .WithDescription("Gets a game by id and returns it");

        #endregion

        group.MapPost("/", async Task<CreatedAtRoute<GameDtoV1>> (IGamesRepository repository,
            CreateGameDto CreateGameDto) =>
        {
            var game = CreateGameDto.AsEntity();
            await repository.CreateAsync(game);
            return TypedResults.CreatedAtRoute(game.AsDtoV1(), GetGameV1EndPointName, new { id = game.Id });
        })
        .MapToApiVersion(1.0)
        .RequireAuthorization(Policies.WriteAccess)
        .WithSummary("Create a game")
        .WithDescription("Creates a new game and returns it");

        group.MapPut("/{id}", async Task<Results<NoContent, NotFound>> (IGamesRepository repository, int id, UpdateGameDto updateGameDto) =>
        {
            var existingGame = await repository.GetByIdAsync(id);

            if (existingGame is null) return TypedResults.NotFound();

            existingGame.Name = updateGameDto.Name;
            existingGame.Genre = updateGameDto.Genre;
            existingGame.Price = updateGameDto.Price;
            existingGame.ReleaseDate = updateGameDto.ReleaseDate;
            existingGame.ImageUri = updateGameDto.ImageUri;

            await repository.UpdateAsync(existingGame);
            return TypedResults.NoContent();
        })
        .MapToApiVersion(1.0)
        .RequireAuthorization(Policies.WriteAccess)
        .WithSummary("Update a game")
        .WithDescription("Updates all game properties");

        group.MapDelete("/{id}", async Task<Results<NoContent, NotFound>> (IGamesRepository repository, int id) =>
        {
            var game = await repository.GetByIdAsync(id);

            if (game is null) return TypedResults.NotFound();

            await repository.DeleteAsync(id);
            return TypedResults.NoContent();
        })
        .MapToApiVersion(1.0)
        .RequireAuthorization(Policies.WriteAccess)
        .WithSummary("Delete a game")
        .WithDescription("Deletes a game by id");

        return group;
    }
}
