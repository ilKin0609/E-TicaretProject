using E_Ticaret_Project.Application.DTOs.UserAuthenticationDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.UserAuthenticationValidations;

public class UserRegisterDtoValidator:AbstractValidator<UserRegisterDto>
{
    public UserRegisterDtoValidator()
    {
        RuleFor(U => U.FullName)
             .NotEmpty().WithMessage("Fullname is required")
             .MaximumLength(100).WithMessage("Fullname cannot exceed 100 characters");

        RuleFor(U => U.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(U => U.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");

        RuleFor(U => U.Role)
           .NotEmpty().WithMessage("Please select role")
           .IsInEnum().WithMessage("Wrong format");
    }
}
