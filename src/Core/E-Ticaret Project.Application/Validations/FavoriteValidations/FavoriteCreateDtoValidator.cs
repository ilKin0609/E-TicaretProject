using E_Ticaret_Project.Application.DTOs.FavoriteDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.FavoriteValidations;

public class FavoriteCreateDtoValidator:AbstractValidator<FavoriteCreateDto>
{
    public FavoriteCreateDtoValidator()
    {
        RuleFor(F => F.ProductId)
            .NotEmpty().WithMessage("ProductId is required.");

        RuleFor(F => F.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .MaximumLength(450).WithMessage("UserId must be at most 450 characters.");
    }
}
