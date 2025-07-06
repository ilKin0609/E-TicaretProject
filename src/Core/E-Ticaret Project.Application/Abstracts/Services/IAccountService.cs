using E_Ticaret_Project.Application.DTOs.UserDtos;
using E_Ticaret_Project.Application.Shared.Responses;

namespace E_Ticaret_Project.Application.Abstracts.Services;

public interface IAccountService
{
    Task<BaseResponse<string>> AddRole(UserAddRoleDto dto);
    Task<BaseResponse<List<UserGetDto>>> GetAll();
    Task<BaseResponse<UserGetDto>> GetById(string userId);
    Task<BaseResponse<string>> UserCreate(UserCreateDto dto);
}
