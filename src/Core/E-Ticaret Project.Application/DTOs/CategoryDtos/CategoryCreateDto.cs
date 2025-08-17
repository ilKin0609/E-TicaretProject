namespace E_Ticaret_Project.Application.DTOs.CategoryDtos;

public record CategoryCreateDto(

    string NameAz,
    string NameRu,
    string NameEn,
    Guid? ParentCategoryId,
 
    string? MetaTitleAz,
    string? MetaTitleRu,
    string? MetaTitleEn,
    string? MetaDescriptionAz,
    string? MetaDescriptionRu,
    string? MetaDescriptionEn,
    string? Keywords
);
