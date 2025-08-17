namespace E_Ticaret_Project.Application.DTOs.UserAuthenticationDtos;

public record UserRegisterDto(

    string Name,
    string Surname,
    string Company,
    string Duty,
    string Phone,
    string Email
);
