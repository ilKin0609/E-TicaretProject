using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.AdminDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.AdminValidations;

public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
{
    public UserUpdateDtoValidator(ILocalizationService localizer)
    {
        RuleFor(x => x.userId)
            .NotEmpty().WithMessage(_ => localizer.Get("User_Id_Required"));

        When(x => x.FirstName is not null, () =>
        {
            RuleFor(x => x.FirstName!)
                .MaximumLength(50).WithMessage(_ => localizer.Get("User_FirstName_MaxLength"));
        });

        When(x => x.LastName is not null, () =>
        {
            RuleFor(x => x.LastName!)
                .MaximumLength(50).WithMessage(_ => localizer.Get("User_LastName_MaxLength"));
        });

        When(x => x.Company is not null, () =>
        {
            RuleFor(x => x.Company!)
                .MaximumLength(100).WithMessage(_ => localizer.Get("User_Company_MaxLength"));
        });

        When(x => x.Position is not null, () =>
        {
            RuleFor(x => x.Position!)
                .MaximumLength(100).WithMessage(_ => localizer.Get("User_Position_MaxLength"));
        });

        When(x => x.Phone is not null, () =>
        {
            RuleFor(x => x.Phone!)
                .MaximumLength(32).WithMessage(_ => localizer.Get("User_Phone_MaxLength"));
        });

        When(x => x.Email is not null, () =>
        {
            RuleFor(x => x.Email!)
                .EmailAddress().WithMessage(_ => localizer.Get("User_Email_Invalid"));
        });

        When(x => x.Login is not null, () =>
        {
            RuleFor(x => x.Login!)
                .MinimumLength(3).WithMessage(_ => localizer.Get("User_Login_MinLength"))
                .MaximumLength(50).WithMessage(_ => localizer.Get("User_Login_MaxLength"));
        });

        When(x => x.Password is not null, () =>
        {
            RuleFor(x => x.Password!)
                .MinimumLength(8).WithMessage(_ => localizer.Get("User_Password_MinLength"))
                .Matches("[A-Z]").WithMessage(_ => localizer.Get("User_Password_Uppercase"))
                .Matches("[a-z]").WithMessage(_ => localizer.Get("User_Password_Lowercase"))
                .Matches("[0-9]").WithMessage(_ => localizer.Get("User_Password_Digit"))
                .Matches("[^a-zA-Z0-9]").WithMessage(_ => localizer.Get("User_Password_Special"));
        });
        // RequestAt üçün ayrıca qayda gərək deyil — DateOnly? gəlirsə qəbul olunur
    }
}
