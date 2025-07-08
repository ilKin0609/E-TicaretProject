namespace E_Ticaret_Project.Application.DTOs.CategoryDtos;

public record CategoryGetDto
(


    Guid Id,
    string Name,
    Guid? ParentCategoryId,
    string? ParentCategoryName,
    List<SubCategoryDto>? SubCategories
);
public record SubCategoryDto
(
    Guid Id,
    string Name
);
