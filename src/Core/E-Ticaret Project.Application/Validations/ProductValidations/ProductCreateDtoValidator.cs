using E_Ticaret_Project.Application.Abstracts.Repositories;
using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.ProductDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.ProductValidations;

public class ProductCreateDtoValidator : AbstractValidator<ProductCreateDto>
{
    public ProductCreateDtoValidator(ILocalizationService localizer)
    {

        RuleFor(x => x.TitleAz)
            .NotEmpty().WithMessage(_ => localizer.Get("Product_TitleAz_Required"))
            .MaximumLength(200).WithMessage(_ => localizer.Get("Product_TitleAz_MaxLength"));

        RuleFor(x => x.SKU)
            .NotEmpty().WithMessage(_ => localizer.Get("Product_SKU_Required"))
            .MaximumLength(64).WithMessage(_ => localizer.Get("Product_SKU_MaxLength"));


        

    }
}

