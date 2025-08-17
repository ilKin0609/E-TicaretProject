using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.ProductDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.ProductValidations;

public class ImageReorderDtoValidator:AbstractValidator<ImageReorderDto>
{
    public ImageReorderDtoValidator(ILocalizationService L)
    {
        RuleFor(x => x.ImageId)
            .NotEmpty().WithMessage(_ => L.Get("ImageReorder_ImageId_Required"));

        // Eyni qaydanı CategoryReorder-dakı kimi saxlayaq: 0..100
        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage(_ => L.Get("ImageReorder_SortOrder_Min"))
            .LessThanOrEqualTo(100).WithMessage(_ => L.Get("ImageReorder_SortOrder_Max"));
    }
}
