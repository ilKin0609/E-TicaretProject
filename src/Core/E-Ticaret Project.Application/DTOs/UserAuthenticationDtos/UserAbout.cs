using E_Ticaret_Project.Application.DTOs.FavoriteDtos;
using E_Ticaret_Project.Application.DTOs.OrderDtos;
using E_Ticaret_Project.Application.DTOs.ProductDtos;
using E_Ticaret_Project.Domain.Enums;

namespace E_Ticaret_Project.Application.DTOs.UserAuthenticationDtos;

public record UserAbout(

    string Id,
    string FullName,
    string Email,
    string? ProfileImageUrl,
    RoleEnum Role,
    List<OrderGetDto>? Buyers,
    List<ProductGetDto>? Sellers,
    List<FavoriteGetDto>? Favorites
);
