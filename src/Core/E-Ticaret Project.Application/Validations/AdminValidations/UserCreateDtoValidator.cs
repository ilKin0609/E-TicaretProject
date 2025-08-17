using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.AdminDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.AdminValidations;

public class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
{
    public UserCreateDtoValidator(ILocalizationService localizer)
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(_ => localizer.Get("User_FirstName_Required"))
            .MaximumLength(50).WithMessage(_ => localizer.Get("User_FirstName_MaxLength"));

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(_ => localizer.Get("User_LastName_Required"))
            .MaximumLength(50).WithMessage(_ => localizer.Get("User_LastName_MaxLength"));

        RuleFor(x => x.Company)
            .NotEmpty().WithMessage(_ => localizer.Get("User_Company_Required"))
            .MaximumLength(100).WithMessage(_ => localizer.Get("User_Company_MaxLength"));

        RuleFor(x => x.Position)
            .NotEmpty().WithMessage(_ => localizer.Get("User_Position_Required"))
            .MaximumLength(100).WithMessage(_ => localizer.Get("User_Position_MaxLength"));

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage(_ => localizer.Get("User_Phone_Required"))
            .MaximumLength(32).WithMessage(_ => localizer.Get("User_Phone_MaxLength"));
        // İstəsən format qaydası da əlavə edə bilərsən

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(_ => localizer.Get("User_Email_Required"))
            .EmailAddress().WithMessage(_ => localizer.Get("User_Email_Invalid"));

        RuleFor(x => x.Login)
            .NotEmpty().WithMessage(_ => localizer.Get("User_Login_Required"))
            .MinimumLength(3).WithMessage(_ => localizer.Get("User_Login_MinLength"))
            .MaximumLength(50).WithMessage(_ => localizer.Get("User_Login_MaxLength"));

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(_ => localizer.Get("User_Password_Required"))
            .MinimumLength(8).WithMessage(_ => localizer.Get("User_Password_MinLength"))
            .Matches("[A-Z]").WithMessage(_ => localizer.Get("User_Password_Uppercase"))
            .Matches("[a-z]").WithMessage(_ => localizer.Get("User_Password_Lowercase"))
            .Matches("[0-9]").WithMessage(_ => localizer.Get("User_Password_Digit"))
            .Matches("[^a-zA-Z0-9]").WithMessage(_ => localizer.Get("User_Password_Special"));
    }
}
