using E_Ticaret_Project.Application.DTOs.ProductDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.ProductValidations;

public class ProductGetDtoValidator:AbstractValidator<ProductGetDto>
{
    public ProductGetDtoValidator()
    {
        RuleFor(P=>P.Id)
            .NotEmpty().WithMessage("Id cannot be null");
    }
}
