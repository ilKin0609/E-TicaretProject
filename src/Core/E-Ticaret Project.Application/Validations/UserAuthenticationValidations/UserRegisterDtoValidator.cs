using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.UserAuthenticationDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.UserAuthenticationValidations;

public class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
{
    private const string PhonePattern = @"^\+?[0-9\s\-]{7,20}$";

    public UserRegisterDtoValidator(ILocalizationService localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(_ => localizer.Get("User_Name_Required"))
            .MaximumLength(100).WithMessage(_ => localizer.Get("User_Name_MaxLength"));

        
        RuleFor(x => x.Surname)
            .NotEmpty().WithMessage(_ => localizer.Get("User_Surname_Required"))
            .MaximumLength(100).WithMessage(_ => localizer.Get("User_Surname_MaxLength"));

        RuleFor(x => x.Company)
            .NotEmpty().WithMessage(_ => localizer.Get("User_Company_Required"))
            .MaximumLength(150).WithMessage(_ => localizer.Get("User_Company_MaxLength"));

        RuleFor(x => x.Duty)
            .NotEmpty().WithMessage(_ => localizer.Get("User_Duty_Required"))
            .MaximumLength(100).WithMessage(_ => localizer.Get("User_Duty_MaxLength"));

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage(_ => localizer.Get("User_Phone_Required"))
            .Matches(PhonePattern).WithMessage(_ => localizer.Get("User_Phone_Invalid"));

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(_ => localizer.Get("User_Email_Required"))
            .EmailAddress().WithMessage(_ => localizer.Get("User_Email_Invalid"))
            .MaximumLength(200).WithMessage(_ => localizer.Get("User_Email_MaxLength"));
    }
}
