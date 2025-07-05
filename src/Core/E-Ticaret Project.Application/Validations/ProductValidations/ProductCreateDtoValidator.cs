using E_Ticaret_Project.Application.DTOs.ProductDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.ProductValidations;

public class ProductCreateDtoValidator : AbstractValidator<ProductCreateDto>
{
    public ProductCreateDtoValidator()
    {
        RuleFor(p => p.Tittle)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

        RuleFor(p => p.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

        RuleFor(p => p.Price)
            .NotEmpty().WithMessage("Price is required.")
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(p => p.Discount)
            .GreaterThanOrEqualTo(0).When(p => p.Discount.HasValue)
            .WithMessage("Discount cannot be negative.");

        RuleFor(p => p.Stock)
            .NotEmpty()
            .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative.");

        RuleFor(p => p.CategoryId)
            .NotEmpty().WithMessage("Category is required.");

    }
}

