using E_Ticaret_Project.Application.DTOs.CategoryDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.CategoryValidations;

public class CategoryGetDtoValidator:AbstractValidator<CategoryGetDto>
{
    public CategoryGetDtoValidator()
    {
        RuleFor(Ct=>Ct.Id)
            .NotEmpty()
            .WithMessage("Id cannot be null");
    }
}
