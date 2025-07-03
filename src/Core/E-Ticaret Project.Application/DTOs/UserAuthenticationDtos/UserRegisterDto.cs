namespace E_Ticaret_Project.Application.DTOs.UserAuthenticationDtos;

public record UserRegisterDto(

    string FullName,
    string Email,
    string? ProfileImageUrl,
    string Password,
    string Role
);
