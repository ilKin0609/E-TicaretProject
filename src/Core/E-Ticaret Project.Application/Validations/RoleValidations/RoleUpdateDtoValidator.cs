using E_Ticaret_Project.Application.DTOs.RoleDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.RoleValidations;

public class RoleUpdateDtoValidator : AbstractValidator<RoleUpdateDto>
{
    public RoleUpdateDtoValidator()
    {
        RuleFor(r => r.RoleId)
            .NotEmpty().WithMessage("RoleId cannot be null");

        RuleFor(r => r)
          .Must(dto =>
              !string.IsNullOrWhiteSpace(dto.Name) ||
              (dto.PermissionList != null && dto.PermissionList.Any())
          )
          .WithMessage("At least one of the name or permission list must be filled in.");

        // Optional: Əgər Name varsa, əlavə yoxlamalar
        When(r => !string.IsNullOrWhiteSpace(r.Name), () =>
        {
            RuleFor(r => r.Name)
                .MaximumLength(100)
                .WithMessage("The name is too long");
        });

        // Optional: Əgər PermissionList varsa, hər biri boş olmamalıdır
        When(x => x.PermissionList != null, () =>
        {
            RuleForEach(r => r.PermissionList)
                .NotEmpty()
                .WithMessage("Permission cannot be null");
        });
    }
}
