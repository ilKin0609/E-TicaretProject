using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.RoleDtos;
using E_Ticaret_Project.Application.Shared.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace E_Ticaret_Project.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private IRoleService _roleservice { get; }
        public RolesController(IRoleService roleservice)
        {
            _roleservice = roleservice;
        }
        // GET: api/<RolesController>
        [HttpGet]
        [Authorize(Policy = Permission.Role.GetAllPermissions)]
        public IActionResult GetAllPermissions()
        {
            var permissions = PermissionHelper.GetAllPermissions();
            return Ok(permissions);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var result=await _roleservice.GetAllRoles();
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{RoleId}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string RoleId)
        {
            var result = await _roleservice.RoleGetByIdAsync(RoleId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        [Authorize(Policy = Permission.Role.Create)]
        public async Task<IActionResult> Create([FromBody] RoleCreateDto dto)
        {
            var result = await _roleservice.CreateRole(dto);
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpPut]
        [Authorize(Policy = Permission.Role.Update)]
        public async Task<IActionResult> Update([FromBody] RoleUpdateDto dto)
        {
            var result = await _roleservice.UpdateRole(dto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{RoleName}")]
        [Authorize(Policy = Permission.Role.Delete)]
        public async Task<IActionResult> Delete([FromRoute] string RoleName)
        {
            var result = await _roleservice.DeleteRole(RoleName);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
