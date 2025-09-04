namespace E_Ticaret_Project.Application.DTOs.CategoryDtos;

public record CategoryBreadcrumbItemDto
(
        Guid Id,
        string NameAz,
        string NameEn,
        string NameRu,
        string Slug
);
