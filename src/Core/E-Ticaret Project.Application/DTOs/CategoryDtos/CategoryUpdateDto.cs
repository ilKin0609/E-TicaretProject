namespace E_Ticaret_Project.Application.DTOs.CategoryDtos;

public record CategoryUpdateDto(

    Guid Id,
    string Name,
    Guid? ParentCategoryId
);
