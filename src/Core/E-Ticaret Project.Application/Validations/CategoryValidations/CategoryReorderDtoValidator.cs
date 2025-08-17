using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.CategoryDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.CategoryValidations;

public class CategoryReorderDtoValidator: AbstractValidator<CategoryReorderDto>
{
    public CategoryReorderDtoValidator(ILocalizationService localizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(_ => localizer.Get("Category_Id_Required"));

        RuleFor(x => x.NewOrder)
            .GreaterThanOrEqualTo(0).WithMessage(_ => localizer.Get("Category_Reorder_NewOrder_Min"))
            .LessThanOrEqualTo(100).WithMessage(_ => localizer.Get("Category_Reorder_NewOrder_Max"));
    }
}
