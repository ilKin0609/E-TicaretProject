using E_Ticaret_Project.Domain.Entities;

namespace E_Ticaret_Project.Application.DTOs.CategoryDtos;

public record CategoryGetDto
(


    Guid Id,
    string NameAz,
    string NameRu,
    string NameEn,

    string? Slug,
    string? MetaTitleAz,
    string? MetaTitleRu,
    string? MetaTitleEn,
    string? MetaDescriptionAz,
    string? MetaDescriptionRu,
    string? MetaDescriptionEn,
    string? Keywords,
    int Order,

    Guid? ParentCategoryId,
    string? ParentCategoryName,
    List<SubCategoryDto>? SubCategories
);
public record SubCategoryDto
(
    Guid Id,
    string NameAz,
    string NameRu,
    string NameEn,
    string? Slug,
    string? MetaTitleAz,
    string? MetaTitleRu,
    string? MetaTitleEn,
    string? MetaDescriptionAz,
    string? MetaDescriptionRu,
    string? MetaDescriptionEn,
    string? Keywords,
    int Order,
    List<SubCategoryDto>? Children
);
