using E_Ticaret_Project.Application.DTOs.ReviewAndCommentDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.ReviewAndCommentValidations;

public class ReviewAndCommentCreateDtoValidator:AbstractValidator<ReviewAndCommentCreateDto>
{
    public ReviewAndCommentCreateDtoValidator()
    {
        RuleFor(Rc => Rc.Comment)
            .NotEmpty()
            .WithMessage("Comment cannot be null");

        RuleFor(Rc => Rc.ProductId)
            .NotEmpty()
            .WithMessage("ProductId cannot be null");

        RuleFor(Rc => Rc.UserId)
            .NotEmpty()
            .WithMessage("UserId cannot be null");
    }
}
