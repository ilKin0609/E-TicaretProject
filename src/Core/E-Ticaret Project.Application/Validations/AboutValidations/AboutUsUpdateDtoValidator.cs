using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.AboutUsDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.AboutValidations;

public class AboutUsUpdateDtoValidator:AbstractValidator<AboutUsUpdateDto>
{
    public AboutUsUpdateDtoValidator(ILocalizationService localizer)
    {
        RuleFor(x => x.MetaTitle_Az)
            .MaximumLength(70)
            .When(x => !string.IsNullOrWhiteSpace(x.MetaTitle_Az))
            .WithMessage(_ => localizer.Get("MetaTitleAZ_MaxLength"));

        RuleFor(x => x.MetaTitle_En)
            .MaximumLength(70)
            .When(x => !string.IsNullOrWhiteSpace(x.MetaTitle_En))
            .WithMessage(_ => localizer.Get("MetaTitleEN_MaxLength"));

        RuleFor(x => x.MetaTitle_Ru)
            .MaximumLength(70)
            .When(x => !string.IsNullOrWhiteSpace(x.MetaTitle_Ru))
            .WithMessage(_ => localizer.Get("MetaTitleRU_MaxLength"));

        RuleFor(x => x.MetaDescription_Az)
            .MaximumLength(160)
            .When(x => !string.IsNullOrWhiteSpace(x.MetaDescription_Az))
            .WithMessage(_ => localizer.Get("MetaDescriptionAZ_MaxLength"));

        RuleFor(x => x.MetaDescription_En)
            .MaximumLength(160)
            .When(x => !string.IsNullOrWhiteSpace(x.MetaDescription_En))
            .WithMessage(_ => localizer.Get("MetaDescriptionEN_MaxLength"));

        RuleFor(x => x.MetaDescription_Ru)
            .MaximumLength(160)
            .When(x => !string.IsNullOrWhiteSpace(x.MetaDescription_Ru))
            .WithMessage(_ => localizer.Get("MetaDescriptionRU_MaxLength"));

        RuleFor(x => x.Keywords)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Keywords))
            .WithMessage(_ => localizer.Get("Keywords_MaxLength"));

        RuleFor(x => x.TitleAZ)
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.TitleAZ))
            .WithMessage(_ => localizer.Get("TitleAZ_MaxLength"));

        RuleFor(x => x.TitleEN)
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.TitleEN))
            .WithMessage(_ => localizer.Get("TitleEN_MaxLength"));

        RuleFor(x => x.TitleRU)
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.TitleRU))
            .WithMessage(_ => localizer.Get("TitleEN_MaxLength"));

        RuleFor(x => x.DescriptionAZ)
            .MaximumLength(3000)
            .When(x => !string.IsNullOrWhiteSpace(x.DescriptionAZ))
            .WithMessage(_ => localizer.Get("DescriptionAZ_MaxLength"));

        RuleFor(x => x.DescriptionEN)
            .MaximumLength(3000)
            .When(x => !string.IsNullOrWhiteSpace(x.DescriptionEN))
            .WithMessage(_ => localizer.Get("DescriptionEN_MaxLength"));

        RuleFor(x => x.DescriptionRU)
            .MaximumLength(3000)
            .When(x => !string.IsNullOrWhiteSpace(x.DescriptionRU))
            .WithMessage(_ => localizer.Get("DescriptionRU_MaxLength"));
    }
}
