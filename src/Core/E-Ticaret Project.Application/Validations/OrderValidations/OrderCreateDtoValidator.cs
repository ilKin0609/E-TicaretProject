using E_Ticaret_Project.Application.DTOs.OrderDtos;
using E_Ticaret_Project.Application.Validations.OrderItemValidations;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.OrderValidations;

public class OrderCreateDtoValidator:AbstractValidator<OrderCreateDto>
{
    public OrderCreateDtoValidator()
    {
        RuleFor(O=>O.ShippingAddress)
            .NotEmpty()
            .WithMessage("ShippingAddress cannot be null");

        RuleFor(O => O.ShoppingAddress)
            .NotEmpty()
            .WithMessage("ShoppingAddress cannot be null");

        RuleFor(O => O.PaymentMethod)
            .IsInEnum()
            .WithMessage("Payment method is invalid.");

        RuleFor(O => O.Items)
           .NotEmpty().WithMessage("Order must contain at least one item.");

        RuleForEach(O => O.Items)
            .SetValidator(new OrderItemCreateDtoValidator());
    }
}
