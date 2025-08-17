using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.AboutUsDtos;
using FluentValidation;


namespace E_Ticaret_Project.Application.Validations.AboutValidations;

public class AboutUsUploadImageDtoValidator : AbstractValidator<AboutUsUploadImageDto>
{
    public AboutUsUploadImageDtoValidator(ILocalizationService localizer)
    {
        RuleFor(x => x.ImageFile)
            .Cascade(CascadeMode.Stop)
            .NotNull()
                .WithMessage(_ => localizer.Get("Image_Required"))
            .Must(file => file != null && file.Length > 0)
                .WithMessage(_ => localizer.Get("ImageFile_Empty"))
            .Must(file =>
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(file.FileName).ToLower();
                return allowedExtensions.Contains(extension);
            })
                .WithMessage(_ => localizer.Get("ImageFile_InvalidExtension"))
            .Must(file => file.Length <= 5 * 1024 * 1024)
                .WithMessage(_ => localizer.Get("ImageFile_TooLarge"));
    }
}
