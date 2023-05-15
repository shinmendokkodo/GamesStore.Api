using GamesStore.Api.Models.Dtos;
using GamesStore.Api.Models.Entities;

namespace GamesStore.Api.Extensions.Models;

public static class EntityExtensions
{
    public static GameDto AsDto(this Game game)
    {
        return new GameDto
        (
            game.Id,
            game.Name,
            game.Genre,
            game.Price,
            game.ReleaseDate,
            game.ImageUri
        );

    }
}