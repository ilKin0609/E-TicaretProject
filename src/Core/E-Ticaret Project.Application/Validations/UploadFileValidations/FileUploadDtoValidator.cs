using E_Ticaret_Project.Application.DTOs.FileUploadDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.UploadFileValidations;

public class FileUploadDtoValidator : AbstractValidator<FileUploadDto>
{
    public FileUploadDtoValidator()
    {
        RuleFor(x => x.File)
            .NotEmpty()
            .WithMessage("You have to upload at least 1 file")
            .Must(file => file != null && file.Length <= 5 * 1024 * 1024)
                .WithMessage("File Size can't be higher than 5 MB");
    }
}
