using E_Ticaret_Project.Application.DTOs.CategoryDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.CategoryValidations;

public class CategoryUpdateDtoValidator : AbstractValidator<CategoryUpdateDto>
{
    public CategoryUpdateDtoValidator()
    {
        RuleFor(Ct => Ct.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(Ct => Ct.Name)
            .NotEmpty().WithMessage("Category name cannot be empty.")
            .MaximumLength(200).WithMessage("Category name must be at most 200 characters.");

        RuleFor(Ct => Ct.ParentCategoryId)
            .NotEqual(Guid.Empty).When(Ct => Ct.ParentCategoryId.HasValue)
            .WithMessage("ParentCategoryId must be a valid GUID.");

        RuleFor(Ct => Ct)
            .Must(Ct => Ct.Id != Ct.ParentCategoryId)
            .WithMessage("A category cannot be its own parent.");
    }
}
