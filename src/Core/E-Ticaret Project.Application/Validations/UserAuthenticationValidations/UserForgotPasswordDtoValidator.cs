using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.UserAuthenticationDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.UserAuthenticationValidations;

public class UserForgotPasswordDtoValidator:AbstractValidator<UserForgotPasswordDto>
{
    public UserForgotPasswordDtoValidator(ILocalizationService localizer)
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(_ => localizer.Get("User_Email_Required"))
            .EmailAddress().WithMessage(_ => localizer.Get("User_Email_Invalid"))
            .MaximumLength(200).WithMessage(_ => localizer.Get("User_Email_MaxLength"));
    }
}
