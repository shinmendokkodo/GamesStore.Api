namespace GamesStore.Api.Models.Dtos;

public record GetGamesDtoV2
(
    int PageNumber = 1,
    int PageSize = 5
);
