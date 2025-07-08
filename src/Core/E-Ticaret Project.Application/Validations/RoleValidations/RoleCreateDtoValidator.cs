using E_Ticaret_Project.Application.DTOs.RoleDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.RoleValidations;

public class RoleCreateDtoValidator : AbstractValidator<RoleCreateDto>
{
    public RoleCreateDtoValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty().WithMessage("Role cannot be null")
            .MaximumLength(100).WithMessage("Role name is long");

        RuleForEach(r => r.PermissionList)
            .NotEmpty().WithMessage("Permission cannot be null")
            .When(r => r.PermissionList != null);
        ;
    }

}
