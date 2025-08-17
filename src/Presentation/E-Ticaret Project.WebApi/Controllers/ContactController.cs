using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.ContactInfoDtos;
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
    public class ContactController : ControllerBase
    {
        private IContactInfoService _contactService { get; }
        public ContactController(IContactInfoService contactService)
        {
            _contactService = contactService;
        }
        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<ContactInfoGetDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get()
        {
            var result= await _contactService.GetContact();
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        [Authorize(Policy = Permission.ContactInfo.Update)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Update([FromBody] ContactInfoUpdateDto dto)
        {
            var result = await _contactService.UpdateAsync(dto);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
