namespace E_Ticaret_Project.Application.DTOs.ProductDtos;

public record ProductImageDto(

     Guid Id,
    string Url,
    bool IsMain,
    int SortOrder,
    string? AltAz,
    string? AltEn,
    string? AltRu
);
