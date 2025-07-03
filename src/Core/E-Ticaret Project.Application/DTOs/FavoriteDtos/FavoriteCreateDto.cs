namespace E_Ticaret_Project.Application.DTOs.FavoriteDtos;

public record FavoriteCreateDto(

    Guid ProductId,
    string UserId
);
