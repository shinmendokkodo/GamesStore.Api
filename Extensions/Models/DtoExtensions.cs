using GamesStore.Api.Models.Dtos;
using GamesStore.Api.Models.Entities;

namespace GamesStore.Api.Extensions.Models;

public static class DtoExtensions
{
    public static Game AsEntity(this CreateGameDto createGameDto)
    {
        return new Game
        {
            Name = createGameDto.Name,
            Genre = createGameDto.Genre,
            Price = createGameDto.Price,
            ReleaseDate = createGameDto.ReleaseDate,
            ImageUri = createGameDto.ImageUri
        };
    }
}
