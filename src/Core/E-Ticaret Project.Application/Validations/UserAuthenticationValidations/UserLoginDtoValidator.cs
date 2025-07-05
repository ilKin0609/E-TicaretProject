using E_Ticaret_Project.Application.DTOs.UserAuthenticationDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.UserAuthenticationValidations;

public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
{
    public UserLoginDtoValidator()
    {
        RuleFor(U => U.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(U => U.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters");
    }
}
