using E_Ticaret_Project.Application.Abstracts.Repositories;
using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.ProductDtos;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace E_Ticaret_Project.Application.Validations.ProductValidations;

public class ProductMainImageUploadDtoValidator : AbstractValidator<ProductMainImageUploadDto>
{
    

    public ProductMainImageUploadDtoValidator(ILocalizationService L)
    {
        RuleFor(x => x.ProductId).NotEmpty()
            .WithMessage(_ => L.Get("Product_Id_Required"));

        RuleFor(x => x.File).NotNull().WithMessage(_ => L.Get("Image_File_Required"));
        RuleFor(x => x.File!.Length)
            .LessThanOrEqualTo(5 * 1024 * 1024).WithMessage(_ => L.Get("Image_File_Max5MB"));
        RuleFor(x => x.File!)
            .Must(f =>
            {
                var ext = Path.GetExtension(f.FileName)?.ToLowerInvariant();
                return ext is ".jpg" or ".jpeg" or ".png" or ".webp";
            })
            .WithMessage(_ => L.Get("Image_File_Ext"));
    }
}
