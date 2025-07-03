namespace E_Ticaret_Project.Application.DTOs.CategoryDtos;

public record CategoryCreateDto(

    string Name,
    Guid? ParentCategoryId
);
