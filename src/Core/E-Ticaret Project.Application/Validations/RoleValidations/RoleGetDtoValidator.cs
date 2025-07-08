using E_Ticaret_Project.Application.DTOs.RoleDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.RoleValidations;

public class RoleGetDtoValidator : AbstractValidator<RoleGetDto>
{
    public RoleGetDtoValidator()
    {
        RuleFor(r => r.RoleId)
            .NotEmpty()
            .WithMessage("RoleId cannot be null");
    }
}
