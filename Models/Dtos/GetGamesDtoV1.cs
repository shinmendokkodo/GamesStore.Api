namespace GamesStore.Api.Models.Dtos;

public record GetGamesDtoV1
(
    int PageNumber = 1,
    int PageSize = 5
);
