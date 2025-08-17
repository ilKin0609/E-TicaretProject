using E_Ticaret_Project.Application.Abstracts.Repositories;
using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.ProductDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.ProductValidations;

public class ProductUpdateDtoValidator : AbstractValidator<ProductUpdateDto>
{
    public ProductUpdateDtoValidator(
        ILocalizationService localizer,
        ICategoryRepository categoryRepo)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(_ => localizer.Get("Product_Id_Required"));

        // ən azı 1 sahə göndərilməlidir
        RuleFor(x => x)
            .Must(HasAnyField)
            .WithMessage(_ => localizer.Get("Product_Update_AtLeastOneFieldRequired"));

        // SKU (göndərilibsə)
        When(x => x.SKU is not null, () =>
        {
            RuleFor(x => x.SKU!)
                .NotEmpty().WithMessage(_ => localizer.Get("Product_SKU_Required"))
                .MaximumLength(64).WithMessage(_ => localizer.Get("Product_SKU_MaxLength"));
        });

        // Qiymətlər (göndərilibsə)
        When(x => x.PriceAZN.HasValue, () =>
        {
            RuleFor(x => x.PriceAZN!.Value)
                .GreaterThanOrEqualTo(0).WithMessage(_ => localizer.Get("Product_Invalid_Price"));
        });
        When(x => x.PartnerPriceAZN.HasValue, () =>
        {
            RuleFor(x => x.PartnerPriceAZN!.Value)
                .GreaterThanOrEqualTo(0).WithMessage(_ => localizer.Get("Product_Invalid_Price"));
        });

        // Başlıqlar (göndərilibsə)
        When(x => x.TitleAz is not null, () =>
        {
            RuleFor(x => x.TitleAz!)
                .NotEmpty().WithMessage(_ => localizer.Get("Product_TitleAz_Required"))
                .MaximumLength(200).WithMessage(_ => localizer.Get("Product_TitleAz_MaxLength"));
        });
        When(x => x.TitleEn is not null, () =>
            RuleFor(x => x.TitleEn!).MaximumLength(200)
                .WithMessage(_ => localizer.Get("Product_TitleEn_MaxLength")));
        When(x => x.TitleRu is not null, () =>
            RuleFor(x => x.TitleRu!).MaximumLength(200)
                .WithMessage(_ => localizer.Get("Product_TitleRu_MaxLength")));

        // Meta (göndərilibsə)
        When(x => x.MetaTitleAz is not null, () =>
            RuleFor(x => x.MetaTitleAz!).MaximumLength(60)
                .WithMessage(_ => localizer.Get("Product_MetaTitleAz_MaxLength")));
        When(x => x.MetaTitleEn is not null, () =>
            RuleFor(x => x.MetaTitleEn!).MaximumLength(60)
                .WithMessage(_ => localizer.Get("Product_MetaTitleEn_MaxLength")));
        When(x => x.MetaTitleRu is not null, () =>
            RuleFor(x => x.MetaTitleRu!).MaximumLength(60)
                .WithMessage(_ => localizer.Get("Product_MetaTitleRu_MaxLength")));

        When(x => x.MetaDescriptionAz is not null, () =>
            RuleFor(x => x.MetaDescriptionAz!).MaximumLength(160)
                .WithMessage(_ => localizer.Get("Product_MetaDescriptionAz_MaxLength")));
        When(x => x.MetaDescriptionEn is not null, () =>
            RuleFor(x => x.MetaDescriptionEn!).MaximumLength(160)
                .WithMessage(_ => localizer.Get("Product_MetaDescriptionEn_MaxLength")));
        When(x => x.MetaDescriptionRu is not null, () =>
            RuleFor(x => x.MetaDescriptionRu!).MaximumLength(160)
                .WithMessage(_ => localizer.Get("Product_MetaDescriptionRu_MaxLength")));

        // Slug (göndərilibsə)
        When(x => x.SlugAz is not null, () =>
            RuleFor(x => x.SlugAz!)
                .MaximumLength(150).WithMessage(_ => localizer.Get("Product_SlugAz_MaxLength")));

        // Tags (göndərilibsə)
        When(x => x.Tags is not null, () =>
        {
            RuleForEach(x => x.Tags!)
                .Must(t => !string.IsNullOrWhiteSpace(t))
                .WithMessage(_ => localizer.Get("Product_Tag_NotEmpty"))
                .MaximumLength(50).WithMessage(_ => localizer.Get("Product_Tag_MaxLength"));
        });
    }

    private static bool HasAnyField(ProductUpdateDto x) =>
        x.CategoryId.HasValue ||
        x.SKU is not null ||
        x.PriceAZN.HasValue ||
        x.PartnerPriceAZN.HasValue ||
        x.TitleAz is not null ||
        x.TitleEn is not null ||
        x.TitleRu is not null ||
        x.DescAz is not null ||
        x.DescEn is not null ||
        x.DescRu is not null ||
        x.MetaTitleAz is not null ||
        x.MetaTitleEn is not null ||
        x.MetaTitleRu is not null ||
        x.MetaDescriptionAz is not null ||
        x.MetaDescriptionEn is not null ||
        x.MetaDescriptionRu is not null ||
        x.SlugAz is not null ||
        x.Tags is not null;
}
