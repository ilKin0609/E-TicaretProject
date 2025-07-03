namespace E_Ticaret_Project.Application.DTOs.FavoriteDtos;

public record FavoriteGetDto(

    Guid Id,
    ProductDto Product,
    string UserId,
    string UserName
);
public record ProductDto
(
    Guid Id,
    string Title,
    decimal Price
);