using E_Ticaret_Project.Application.DTOs.SiteSettingDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.SiteSettingValidations;

public class SiteSettingUpdateDtoValidator : AbstractValidator<SiteSettingUpdateDto>
{
    public SiteSettingUpdateDtoValidator()
    {
        RuleFor(x => x.ScrollTextSpeed)
            .InclusiveBetween(1, 100)
            .When(x => x.ScrollTextSpeed.HasValue)
            .WithMessage("Scroll speed 1 ilə 100 arasında olmalıdır.");

        RuleFor(x => x.PublicEmail)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.PublicEmail))
            .WithMessage("Public email düzgün deyil.");

        RuleFor(x => x.WhatsappInquiryLink)
            .Must(BeValidUrl).When(x => !string.IsNullOrWhiteSpace(x.WhatsappInquiryLink))
            .WithMessage("Whatsapp link düzgün URL deyil.");

        RuleFor(x => x.FacebookUrl)
            .Must(BeValidUrl).When(x => !string.IsNullOrWhiteSpace(x.FacebookUrl))
            .WithMessage("Facebook link düzgün URL deyil.");

        RuleFor(x => x.InstagramUrl)
            .Must(BeValidUrl).When(x => !string.IsNullOrWhiteSpace(x.InstagramUrl))
            .WithMessage("Instagram link düzgün URL deyil.");

        RuleFor(x => x.YouTubeUrl)
            .Must(BeValidUrl).When(x => !string.IsNullOrWhiteSpace(x.YouTubeUrl))
            .WithMessage("YouTube link düzgün URL deyil.");

        RuleFor(x => x.HomeMetaTitleAz)
             .MaximumLength(70).WithMessage("Ana səhifə (AZ) meta title maksimum 70 simvol ola bilər.");
        RuleFor(x => x.HomeMetaTitleRu)
            .MaximumLength(70).WithMessage("Главная (RU) meta title максимум 70 символов.");
        RuleFor(x => x.HomeMetaTitleEn)
            .MaximumLength(70).WithMessage("Home (EN) meta title can be at most 70 characters.");

        // Meta description: max 160
        RuleFor(x => x.HomeMetaDescriptionAz)
            .MaximumLength(160).WithMessage("Ana səhifə (AZ) meta description maksimum 160 simvol ola bilər.");
        RuleFor(x => x.HomeMetaDescriptionRu)
            .MaximumLength(160).WithMessage("Главная (RU) meta description максимум 160 символов.");
        RuleFor(x => x.HomeMetaDescriptionEn)
            .MaximumLength(160).WithMessage("Home (EN) meta description can be at most 160 characters.");
    }

    private bool BeValidUrl(string? url)
        => Uri.TryCreate(url, UriKind.Absolute, out var u) &&
           (u.Scheme == Uri.UriSchemeHttp || u.Scheme == Uri.UriSchemeHttps);
}
