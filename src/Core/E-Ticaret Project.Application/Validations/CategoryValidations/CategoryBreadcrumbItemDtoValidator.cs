using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.CategoryDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.CategoryValidations;

public class CategoryBreadcrumbItemDtoValidator:AbstractValidator<CategoryBreadcrumbItemDto>
{
    public CategoryBreadcrumbItemDtoValidator(ILocalizationService localizer)
    {
        RuleFor(ct=>ct.Id)
            .NotEmpty()
            .WithMessage(_ => localizer.Get("Category_Id_Required"));
    }
}
