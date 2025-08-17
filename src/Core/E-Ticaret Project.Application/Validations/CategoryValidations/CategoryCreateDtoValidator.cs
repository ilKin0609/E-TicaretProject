using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.CategoryDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.CategoryValidations;

public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
{
    public CategoryCreateDtoValidator(ILocalizationService localizer)
    {
        RuleFor(x => x.NameAz)
            .NotEmpty().WithMessage(_ => localizer.Get("Category_NameAz_Required"))
            .MaximumLength(200).WithMessage(_ => localizer.Get("Category_NameAz_MaxLength"));

        RuleFor(x => x.NameRu)
            .NotEmpty().WithMessage(_ => localizer.Get("Category_NameRu_Required"))
            .MaximumLength(200).WithMessage(_ => localizer.Get("Category_NameRu_MaxLength"));

        RuleFor(x => x.NameEn)
            .NotEmpty().WithMessage(_ => localizer.Get("Category_NameEn_Required"))
            .MaximumLength(200).WithMessage(_ => localizer.Get("Category_NameEn_MaxLength"));

        RuleFor(x => x.MetaTitleAz).MaximumLength(255)
            .WithMessage(_ => localizer.Get("Category_MetaTitleAz_MaxLength"));
        RuleFor(x => x.MetaTitleRu).MaximumLength(255)
            .WithMessage(_ => localizer.Get("Category_MetaTitleRu_MaxLength"));
        RuleFor(x => x.MetaTitleEn).MaximumLength(255)
            .WithMessage(_ => localizer.Get("Category_MetaTitleEn_MaxLength"));

        RuleFor(x => x.MetaDescriptionAz).MaximumLength(1000)
            .WithMessage(_ => localizer.Get("Category_MetaDescriptionAz_MaxLength"));
        RuleFor(x => x.MetaDescriptionRu).MaximumLength(1000)
            .WithMessage(_ => localizer.Get("Category_MetaDescriptionRu_MaxLength"));
        RuleFor(x => x.MetaDescriptionEn).MaximumLength(1000)
            .WithMessage(_ => localizer.Get("Category_MetaDescriptionEn_MaxLength"));

        RuleFor(x => x.Keywords).MaximumLength(500)
            .WithMessage(_ => localizer.Get("Category_Keywords_MaxLength"));
    }
}
