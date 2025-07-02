using E_Ticaret_Project.Application.DTOs.CategoryDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.CategoryValidations;

public class CategoryCreateDtoValidator:AbstractValidator<CategoryCreateDto>
{
    public CategoryCreateDtoValidator() { }
}
