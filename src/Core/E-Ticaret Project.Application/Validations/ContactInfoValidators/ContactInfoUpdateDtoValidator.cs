using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.ContactInfoDtos;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace E_Ticaret_Project.Application.Validations.ContactInfoValidators;

public class ContactInfoUpdateDtoValidator:AbstractValidator<ContactInfoUpdateDto>
{
    public ContactInfoUpdateDtoValidator(ILocalizationService localizer)
    {
        RuleFor(x => x.Phone)
           .MaximumLength(50)
           .When(x => !string.IsNullOrWhiteSpace(x.Phone))
           .WithMessage(_ => localizer.Get("Contact_Phone_Length"));

        // Email
        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage(_ => localizer.Get("Contact_Email_Invalid"));

        // Google Map
        RuleFor(x => x.MapIframeSrc)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.MapIframeSrc))
            .WithMessage(_ => localizer.Get("Contact_Map_Invalid"));

        // Meta Titles
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

        // Meta Descriptions
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

        // Keywords
        RuleFor(x => x.Keywords)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Keywords))
            .WithMessage(_ => localizer.Get("Keywords_MaxLength"));

        // Titles
        RuleFor(x => x.Title_Az)
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.Title_Az))
            .WithMessage(_ => localizer.Get("TitleAZ_MaxLength"));

        RuleFor(x => x.Title_En)
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.Title_En))
            .WithMessage(_ => localizer.Get("TitleEN_MaxLength"));

        RuleFor(x => x.Title_Ru)
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.Title_Ru))
            .WithMessage(_ => localizer.Get("TitleRU_MaxLength"));

        // Address
        RuleFor(x => x.AddressAZ)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.AddressAZ))
            .WithMessage(_ => localizer.Get("AddressAZ_MaxLength"));

        RuleFor(x => x.AddressEN)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.AddressEN))
            .WithMessage(_ => localizer.Get("AddressEN_MaxLength"));

        RuleFor(x => x.AddressRU)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.AddressRU))
            .WithMessage(_ => localizer.Get("AddressRU_MaxLength"));
    }
}
