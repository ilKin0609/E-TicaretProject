using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.UserDtos;
using E_Ticaret_Project.Application.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace E_Ticaret_Project.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {

        private IAccountService _accountService;
        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // GET: api/<AccountsController>
        [HttpGet]
        [Authorize(Policy = "Account.GetAllUser")]
        [ProducesResponseType(typeof(BaseResponse<List<UserGetDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _accountService.GetAll();
            return StatusCode((int)result.StatusCode, result);
        }

        // GET api/<AccountsController>/5
        [HttpGet("{id}")]
        [Authorize(Policy = "Account.GetUser")]
        [ProducesResponseType(typeof(BaseResponse<UserGetDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _accountService.GetById(id);
            return StatusCode((int)result.StatusCode, result);

        }

        // POST api/<AccountsController>
        [HttpPost]
        [Authorize(Policy = "Account.AddRole")]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> AddRole([FromBody] UserAddRoleDto dto)
        {
            var result= await _accountService.AddRole(dto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        [Authorize(Policy = "Account.Create")]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UserCreate([FromBody] UserCreateDto dto)
        {
            var result = await _accountService.UserCreate(dto);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
