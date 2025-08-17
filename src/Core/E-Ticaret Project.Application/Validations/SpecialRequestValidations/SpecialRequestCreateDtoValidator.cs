using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.SpecialRequestDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.ReviewAndCommentValidations;

public class SpecialRequestCreateDtoValidator : AbstractValidator<SpecialRequestCreateDto>
{

    public SpecialRequestCreateDtoValidator(ILocalizationService localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(_ => localizer.Get("SpecialRequest_Name_Required"))
            .MaximumLength(50)
            .WithMessage(_ => localizer.Get("SpecialRequest_Name_MaxLength"));

        RuleFor(x => x.Surname)
            .NotEmpty()
            .WithMessage(_ => localizer.Get("SpecialRequest_Surname_Required"))
            .MaximumLength(50)
            .WithMessage(_ => localizer.Get("SpecialRequest_Surname_MaxLength"));

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage(_ => localizer.Get("SpecialRequest_Phone_Required"))
            .Matches(@"^((\+994|0)(50|51|55|70|77|99)\d{7}|\+[1-9]\d{1,14})$")
            .WithMessage(_ => localizer.Get("Phone_InvalidFormat"))
            .MinimumLength(10)
            .WithMessage(_ => localizer.Get("Phone_MinLength"))
            .MaximumLength(15)
            .WithMessage(_ => localizer.Get("SpecialRequest_Phone_MaxLength"));

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(_ => localizer.Get("SpecialRequest_Email_Required"))
            .EmailAddress()
            .WithMessage(_ => localizer.Get("SpecialRequest_Email_Invalid"));

        RuleFor(x => x.OrderAbout)
            .NotEmpty()
            .WithMessage(_ => localizer.Get("SpecialRequest_OrderAbout_Required"))
            .MaximumLength(1000)
            .WithMessage(_ => localizer.Get("SpecialRequest_OrderAbout_MaxLength"));

        When(x => x.File != null, () =>
        {
            RuleFor(x => x.File!.Length)
                .LessThanOrEqualTo(10 * 1024 * 1024)
                .WithMessage(_ => localizer.Get("SpecialRequest_File_MaxSize")); // 10 MB

            RuleFor(x => x.File!.FileName)
                .Must(name =>
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf", ".doc", ".docx", ".pptx" };
                    var extension = Path.GetExtension(name).ToLower();
                    return allowedExtensions.Contains(extension);
                })
                .WithMessage(_ => localizer.Get("SpecialRequest_File_InvalidExtension"));
        });
    }
}

