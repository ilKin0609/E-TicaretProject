using E_Ticaret_Project.Application.DTOs.CategoryDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.FavoriteValidations;

public class FavoriteGetDtoValidator : AbstractValidator<CategoryGetDto>
{
    public FavoriteGetDtoValidator()
    {
        RuleFor(F => F.Id)
            .NotEmpty()
            .WithMessage("Id cannot be null");
    }
}
