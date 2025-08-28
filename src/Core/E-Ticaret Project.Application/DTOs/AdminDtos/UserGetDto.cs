namespace E_Ticaret_Project.Application.DTOs.AdminDtos;

public record UserGetDto(

    string userId,
    string FirstName,
    string LastName,
    string Company,
    string Position,
    string Phone,
    string Email,
    string Login,
    string Password,
    DateOnly? RequestAt,
    DateTime? LastLoginAt,
    bool isToggle
    );
