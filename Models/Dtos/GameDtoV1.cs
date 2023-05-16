namespace GamesStore.Api.Models.Dtos;

public record GameDtoV1
(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    DateTime ReleaseDate,
    string ImageUrl
);
