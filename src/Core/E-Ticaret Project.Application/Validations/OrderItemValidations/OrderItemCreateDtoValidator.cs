using E_Ticaret_Project.Application.DTOs.OrderItemDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.OrderItemValidations;

public class OrderItemCreateDtoValidator : AbstractValidator<OrderItemCreateDto>
{
    public OrderItemCreateDtoValidator()
    {
        RuleFor(Oi => Oi.ProductId)
            .NotEmpty().WithMessage("ProductId is required.");

        RuleFor(Oi => Oi.OrderCount)
            .GreaterThan(0).WithMessage("OrderCount must be greater than zero.");
    }
}
