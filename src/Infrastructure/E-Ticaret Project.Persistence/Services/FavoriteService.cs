using E_Ticaret_Project.Application.Abstracts.Repositories;
using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.FavoriteDtos;
using E_Ticaret_Project.Application.Shared.Responses;
using E_Ticaret_Project.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;

namespace E_Ticaret_Project.Persistence.Services;

public class FavoriteService : IFavoriteService
{
    private IFavoriteRepository _favoriteRepository { get; }

    public FavoriteService(IFavoriteRepository favoriteRepository)
    {
        _favoriteRepository = favoriteRepository;
    }
 
    public async Task<BaseResponse<List<FavoriteGetDto>>> MyFavorities(string userId)
    {
        var favorites = await _favoriteRepository
        .GetAllFiltered(
            predicate: f => f.UserId == userId,
            include:
            [
                f => f.Product,
                f => f.User
            ]
            
        )
        .ToListAsync();

        var favoriteDtos = new List<FavoriteGetDto>();

        foreach (var f in favorites)
        {
            favoriteDtos.Add(new FavoriteGetDto(
                f.Id,
                new ProductDto(f.Product.Id, f.Product.Tittle, f.Product.Price),
                f.UserId,
                f.User.FullName));
        }

        return new("Favorites fetched successfully", favoriteDtos, HttpStatusCode.OK);
    }

}
