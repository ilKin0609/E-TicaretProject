using E_Ticaret_Project.Domain.Enums;

namespace E_Ticaret_Project.Application.DTOs.UserDtos;

public record UserCreateDto(

    string FullName,
    string Email,
    string Password,
    RoleAdminEnum Role
);
