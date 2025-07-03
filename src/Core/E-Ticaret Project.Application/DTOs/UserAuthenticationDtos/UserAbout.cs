using E_Ticaret_Project.Application.DTOs.FavoriteDtos;
using E_Ticaret_Project.Application.DTOs.OrderDtos;
using E_Ticaret_Project.Application.DTOs.ProductDtos;

namespace E_Ticaret_Project.Application.DTOs.UserAuthenticationDtos;

public record UserAbout(

    string Id,
    string FullName,
    string Email,
    string? ProfileImageUrl,
    string Role,
    List<OrderGetDto>? Buyyers,
    List<ProductGetDto>? Sellers,
    List<FavoriteGetDto>? Favorites
);
