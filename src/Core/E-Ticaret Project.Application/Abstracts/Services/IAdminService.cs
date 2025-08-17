using E_Ticaret_Project.Application.DTOs.AdminDtos;
using E_Ticaret_Project.Application.Shared.Responses;

namespace E_Ticaret_Project.Application.Abstracts.Services;

public interface IAdminService
{
    Task<BaseResponse<string>> AddRole(UserAddRoleDto dto);
    Task<BaseResponse<List<UserGetDto>>> GetAll();
    Task<BaseResponse<string>> UserCreate(UserCreateDto dto);
    Task<BaseResponse<string>> UserDelete(string userId);
    Task<BaseResponse<string>> ToggleUser(string userId);
    Task<BaseResponse<string>> UnToggleUser(string userId);
    Task<BaseResponse<string>> UserUpdate(UserUpdateDto dto);
}
