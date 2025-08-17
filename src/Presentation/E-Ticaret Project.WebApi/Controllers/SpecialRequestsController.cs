using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.SpecialRequestDtos;
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
    public class SpecialRequestsController : ControllerBase
    {
        private ISpecialRequestService _specialReq { get; }
        public SpecialRequestsController(ISpecialRequestService specialReq)
        {
            _specialReq = specialReq;
        }
 
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateRequest([FromForm] SpecialRequestCreateDto dto)
        {
            var response = await _specialReq.CreateRequest(dto);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpDelete("{imageId}")]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> RemoveImage([FromRoute] Guid imageId)
        {
            var response = await _specialReq.RemoveImageAsync(imageId);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
