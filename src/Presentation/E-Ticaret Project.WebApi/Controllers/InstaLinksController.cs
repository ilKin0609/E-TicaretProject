using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.InstaLinkDtos;
using E_Ticaret_Project.Application.Shared.Permissions;
using E_Ticaret_Project.Application.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace E_Ticaret_Project.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InstaLinksController : ControllerBase
    {
        private IInstaLinkService _serviceInsta { get; }
        public InstaLinksController(IInstaLinkService serviceInsta)
        {
            _serviceInsta = serviceInsta;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts([FromQuery] int count = 6)
        {
            var list = await _serviceInsta.GetRandomAsync(count);
            return Ok(list);
        }


        [HttpPost]
        [Authorize(Policy = Permission.InstaLink.Create)]
        [ProducesResponseType(typeof(BaseResponse<InstagramLinkVm>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Post([FromBody] InstaLinkCreateDto dto)
        {
            var result = await _serviceInsta.AddAsync(dto);
            return StatusCode((int)result.StatusCode,result);
        }

        [HttpDelete]
        [Authorize(Policy = Permission.InstaLink.Delete)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Delete([FromBody] InstagramLinkDeleteDto dto)
        {
            var result = await _serviceInsta.DeleteAsync(dto);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
