using E_Ticaret_Project.Application.DTOs.UserDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.UserValidations;

public class UserGetDtoValidator:AbstractValidator<UserGetDto>
{
    public UserGetDtoValidator()
    {
        RuleFor(U=>U.Id).NotEmpty();
    }
}
