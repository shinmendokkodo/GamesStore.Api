namespace GamesStore.Api.Models.Dtos;

public record GameDto
(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    DateTime ReleaseDate,
    string ImageUrl
);
