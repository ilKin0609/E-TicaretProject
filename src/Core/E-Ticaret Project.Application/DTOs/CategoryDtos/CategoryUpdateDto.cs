namespace E_Ticaret_Project.Application.DTOs.CategoryDtos;

public record CategoryUpdateDto(

        Guid Id,
        string? NameAz,
        string? NameRu,
        string? NameEn,
        Guid? ParentCategoryId,
        bool? IsVisible,
        int? Order,
        string? MetaTitleAz,
        string? MetaTitleRu,
        string? MetaTitleEn,
        string? MetaDescriptionAz,
        string? MetaDescriptionRu,
        string? MetaDescriptionEn,
        string? Keywords
);
