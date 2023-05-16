using GamesStore.Api.Models.Dtos;
using GamesStore.Api.Models.Entities;

namespace GamesStore.Api.Extensions.Models;

public static class EntityExtensions
{
    public static GameDtoV1 AsDtoV1(this Game game)
    {
        return new GameDtoV1
        (
            game.Id,
            game.Name,
            game.Genre,
            game.Price,
            game.ReleaseDate,
            game.ImageUri
        );

    }

    public static GameDtoV2 AsDtoV2(this Game game)
    {
        return new GameDtoV2
        (
            game.Id,
            game.Name,
            game.Genre,
            game.Price - (game.Price * 0.3m),
            game.Price,
            game.ReleaseDate,
            game.ImageUri
        );

    }
}