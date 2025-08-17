using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.CategoryDtos;
using FluentValidation;

namespace E_Ticaret_Project.Application.Validations.CategoryValidations;

public class CategoryUpdateDtoValidator : AbstractValidator<CategoryUpdateDto>
{
    public CategoryUpdateDtoValidator(ILocalizationService localizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(_ => localizer.Get("Category_Id_Required"));

        // Ən azı 1 sahə verilməlidir
        RuleFor(x => x)
            .Must(HasAnyField)
            .WithMessage(_ => localizer.Get("Category_Update_AtLeastOneFieldRequired"));

        // Hər sahə yalnız GÖNDƏRİLİB-sə yoxlanır
        When(x => x.NameAz is not null, () =>
        {
            RuleFor(x => x.NameAz!)
                .NotEmpty().WithMessage(_ => localizer.Get("Category_NameAz_Required"))
                .MaximumLength(200).WithMessage(_ => localizer.Get("Category_NameAz_MaxLength"));
        });

        When(x => x.NameRu is not null, () =>
        {
            RuleFor(x => x.NameRu!)
                .NotEmpty().WithMessage(_ => localizer.Get("Category_NameRu_Required"))
                .MaximumLength(200).WithMessage(_ => localizer.Get("Category_NameRu_MaxLength"));
        });

        When(x => x.NameEn is not null, () =>
        {
            RuleFor(x => x.NameEn!)
                .NotEmpty().WithMessage(_ => localizer.Get("Category_NameEn_Required"))
                .MaximumLength(200).WithMessage(_ => localizer.Get("Category_NameEn_MaxLength"));
        });

        // Özünü parent seçməsin (yalnız parent göndərilibsə)
        When(x => x.ParentCategoryId.HasValue, () =>
        {
            RuleFor(x => x)
                .Must(x => x.ParentCategoryId != x.Id)
                .WithMessage(_ => localizer.Get("Category_Parent_CannotBeSelf"));
        });

        // Order gəlirsə – limitlər
        When(x => x.Order.HasValue, () =>
        {
            RuleFor(x => x.Order!.Value)
                .GreaterThanOrEqualTo(0).WithMessage(_ => localizer.Get("Category_Order_Min"))
                .LessThanOrEqualTo(100).WithMessage(_ => localizer.Get("Category_Order_Max"));
        });

        
        When(x => x.MetaTitleAz is not null, () =>
            RuleFor(x => x.MetaTitleAz!).MaximumLength(255)
                .WithMessage(_ => localizer.Get("Category_MetaTitleAz_MaxLength")));
        When(x => x.MetaTitleRu is not null, () =>
            RuleFor(x => x.MetaTitleRu!).MaximumLength(255)
                .WithMessage(_ => localizer.Get("Category_MetaTitleRu_MaxLength")));
        When(x => x.MetaTitleEn is not null, () =>
            RuleFor(x => x.MetaTitleEn!).MaximumLength(255)
                .WithMessage(_ => localizer.Get("Category_MetaTitleEn_MaxLength")));

        When(x => x.MetaDescriptionAz is not null, () =>
            RuleFor(x => x.MetaDescriptionAz!).MaximumLength(1000)
                .WithMessage(_ => localizer.Get("Category_MetaDescriptionAz_MaxLength")));
        When(x => x.MetaDescriptionRu is not null, () =>
            RuleFor(x => x.MetaDescriptionRu!).MaximumLength(1000)
                .WithMessage(_ => localizer.Get("Category_MetaDescriptionRu_MaxLength")));
        When(x => x.MetaDescriptionEn is not null, () =>
            RuleFor(x => x.MetaDescriptionEn!).MaximumLength(1000)
                .WithMessage(_ => localizer.Get("Category_MetaDescriptionEn_MaxLength")));

        When(x => x.Keywords is not null, () =>
            RuleFor(x => x.Keywords!).MaximumLength(500)
                .WithMessage(_ => localizer.Get("Category_Keywords_MaxLength")));
    }

    private static bool HasAnyField(CategoryUpdateDto x) =>
        x.NameAz != null ||
        x.NameRu != null ||
        x.NameEn != null ||
        x.ParentCategoryId.HasValue ||
        x.IsVisible.HasValue ||
        x.Order.HasValue ||
        x.MetaTitleAz != null ||
        x.MetaTitleRu != null ||
        x.MetaTitleEn != null ||
        x.MetaDescriptionAz != null ||
        x.MetaDescriptionRu != null ||
        x.MetaDescriptionEn != null ||
        x.Keywords != null;
}
