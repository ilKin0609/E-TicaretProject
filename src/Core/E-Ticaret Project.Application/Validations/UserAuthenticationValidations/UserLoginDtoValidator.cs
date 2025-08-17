using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.UserAuthenticationDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.UserAuthenticationValidations;

public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
{
    public UserLoginDtoValidator(ILocalizationService localizer)
    {
        RuleFor(u => u.UserName)
            .NotEmpty()
            .WithMessage(_ => localizer.Get("User_Login_Username_Required"));

        RuleFor(u => u.Password)
            .NotEmpty()
            .WithMessage(_ => localizer.Get("User_Login_Password_Required"))
            .MinimumLength(8)
            .WithMessage(_ => localizer.Get("User_Login_Password_MinLength"));
    }
}
