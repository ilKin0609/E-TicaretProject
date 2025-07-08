using E_Ticaret_Project.Application.DTOs.FavoriteDtos;
using E_Ticaret_Project.Application.Shared.Responses;

namespace E_Ticaret_Project.Application.Abstracts.Services;

public interface IFavoriteService
{
    Task<BaseResponse<List<FavoriteGetDto>>> MyFavorities(string userId);
}
