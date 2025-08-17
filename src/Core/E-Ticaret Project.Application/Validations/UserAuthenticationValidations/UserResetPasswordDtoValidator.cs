using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.UserAuthenticationDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.UserAuthenticationValidations;

public class UserResetPasswordDtoValidator:AbstractValidator<UserResetPasswordDto>
{
    public UserResetPasswordDtoValidator(ILocalizationService localizer)
    {
        RuleFor(rp => rp.UserId)
            .NotEmpty()
            .WithMessage(_ => localizer.Get("User_Reset_UserId_Required"));

        RuleFor(rp => rp.Token)
            .NotEmpty()
            .WithMessage(_ => localizer.Get("User_Reset_Token_Required"));

        RuleFor(rp => rp.NewPassword)
            .NotEmpty().WithMessage(_ => localizer.Get("User_Reset_NewPassword_Required"))
            .MinimumLength(8).WithMessage(_ => localizer.Get("User_Reset_NewPassword_MinLength"))
            .Matches("[A-Z]").WithMessage(_ => localizer.Get("User_Reset_NewPassword_Upper"))
            .Matches("[a-z]").WithMessage(_ => localizer.Get("User_Reset_NewPassword_Lower"))
            .Matches("[0-9]").WithMessage(_ => localizer.Get("User_Reset_NewPassword_Digit"))
            .Matches("[^a-zA-Z0-9]").WithMessage(_ => localizer.Get("User_Reset_NewPassword_Special"));
    }
}
