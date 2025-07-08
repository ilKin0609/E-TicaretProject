using E_Ticaret_Project.Application.DTOs.UserAuthenticationDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.UserAuthenticationValidations;

public class UserForgotPasswordDtoValidator:AbstractValidator<UserForgotPasswordDto>
{
    public UserForgotPasswordDtoValidator()
    {
        RuleFor(Fp => Fp.Email)
            .NotEmpty()
            .WithMessage("Email cannot be null");
    }
}
