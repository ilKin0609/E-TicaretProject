using E_Ticaret_Project.Application.DTOs.InstaLinkDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.InstaLinkValidations;
public sealed class InstagramLinkCreateDtoValidator : AbstractValidator<InstaLinkCreateDto>
{
    public InstagramLinkCreateDtoValidator()
    {
        RuleFor(x => x.Link)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Link boş ola bilməz.")
            .MaximumLength(2048).WithMessage("Link çox uzundur.")
            // xam linki canonical-a çevirmə cəhdi
            .Must(link => InstagramUrlHelper.ToCanonicalPermalink(link) is not null)
                .WithMessage("Keçərli Instagram linki daxil edin (post/reel/tv).");
    }
}