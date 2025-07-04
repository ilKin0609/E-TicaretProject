using E_Ticaret_Project.Domain.Enums;

namespace E_Ticaret_Project.Application.DTOs.UserAuthenticationDtos;

public record UserRegisterDto(

    string FullName,
    string Email,
    string Password,
    RoleEnum Role
);
