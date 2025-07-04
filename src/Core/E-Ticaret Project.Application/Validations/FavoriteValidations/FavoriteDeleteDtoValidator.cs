using E_Ticaret_Project.Application.DTOs.FavoriteDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.FavoriteValidations;

public class FavoriteDeleteDtoValidator:AbstractValidator<FavoriteDeleteDto>
{
    public FavoriteDeleteDtoValidator()
    {
        RuleFor(F => F.Id)
            .NotEmpty()
            .WithMessage("Id cannot be null");
    }
}
