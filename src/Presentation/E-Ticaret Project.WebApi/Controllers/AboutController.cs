using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.AboutUsDtos;
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
    public class AboutController : ControllerBase
    {
        private IAboutService _aboutService { get; }
        public AboutController(IAboutService aboutService)
        {
            _aboutService = aboutService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<AboutUsGetDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get()
        {
            var result = await _aboutService.GetAbout();
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        [Authorize(Policy = Permission.About.UploadImage)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UploadImage([FromForm] AboutUsUploadImageDto dto)
        {
            var result = await _aboutService.AboutUploadImage(dto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        [Authorize(Policy = Permission.About.Update)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Update([FromBody] AboutUsUpdateDto dto)
        {
            var result= await _aboutService.UpdateAsync(dto);
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpDelete]
        [Authorize(Policy = Permission.About.RemoveImage)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> RemoveImage()
        {
            var result = await _aboutService.RemoveImageAsync();
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
