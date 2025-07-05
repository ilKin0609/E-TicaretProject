using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.FavoriteDtos;
using E_Ticaret_Project.Application.Shared.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace E_Ticaret_Project.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FavoritiesController : ControllerBase
    {
        private IFavoriteService _favoriteService { get; }
        public FavoritiesController(IFavoriteService favoriteService) 
        {
            _favoriteService = favoriteService;
        }
        // GET: api/<FavoritiesController>
        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<FavoriteGetDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> MyFavorites(string userId)
        {
            var favorites=await _favoriteService.MyFavorities(userId);
            return StatusCode((int)favorites.StatusCode, favorites);
        }

        // POST api/<FavoritiesController>
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> AddFavorite([FromBody] FavoriteCreateDto dto)
        {
            var result=await _favoriteService.AddFavorite(dto);
            return StatusCode((int)result.StatusCode,result);
        }

        // DELETE api/<FavoritiesController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> RemoveFavorite(Guid id)
        {
            var result = await _favoriteService.RemoveFavorite(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
