namespace E_Ticaret_Project.Application.DTOs.UserAuthenticationDtos;

public record UserResetPasswordDto(

    string UserId,
    string Token, 
    string NewPassword
);
