namespace GamesStore.Api.Models.Dtos;

public record GameDtoV2
(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    decimal RetailPrice,
    DateTime ReleaseDate,
    string ImageUri
);
