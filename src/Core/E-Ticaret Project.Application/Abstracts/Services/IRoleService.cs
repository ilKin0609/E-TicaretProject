using E_Ticaret_Project.Application.DTOs.RoleDtos;
using E_Ticaret_Project.Application.Shared.Responses;

namespace E_Ticaret_Project.Application.Abstracts.Services;

public interface IRoleService
{
    Task<BaseResponse<List<RoleGetDto>>> GetAllRoles();
    Task<BaseResponse<RoleGetDto>> RoleGetByIdAsync(string RoleId);
    Task<BaseResponse<string?>> CreateRole(RoleCreateDto dto);
    Task<BaseResponse<string?>> UpdateRole(RoleUpdateDto dto);
    Task<BaseResponse<string?>> DeleteRole(string RoleName);
}
