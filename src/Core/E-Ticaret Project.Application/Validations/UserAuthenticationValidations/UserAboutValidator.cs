using E_Ticaret_Project.Application.DTOs.UserAuthenticationDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.UserAuthenticationValidations;

public class UserAboutValidator:AbstractValidator<UserAbout>
{
    public UserAboutValidator()
    {
        RuleFor(U => U.Id)
            .NotEmpty()
            .WithMessage("UserId cannot be null");
    }
}
