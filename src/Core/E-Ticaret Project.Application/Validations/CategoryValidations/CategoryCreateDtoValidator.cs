using E_Ticaret_Project.Application.DTOs.CategoryDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.CategoryValidations;

public class CategoryCreateDtoValidator:AbstractValidator<CategoryCreateDto>
{
    public CategoryCreateDtoValidator() {

        RuleFor(Ct => Ct.Name)
            .NotEmpty().WithMessage("Category cannot be null")
            .MaximumLength(100).WithMessage("Category name must be at most 100 characters.");

        RuleFor(Ct => Ct.ParentCategoryId)
            .NotEqual(Guid.Empty).When(Ct => Ct.ParentCategoryId.HasValue)
            .WithMessage("ParentCategoryId must be a valid GUID.");
    }
}
