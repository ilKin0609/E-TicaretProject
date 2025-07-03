namespace E_Ticaret_Project.Application.DTOs.ProductDtos;

public record ProductCreateDto(
    string Tittle,
    string? Description,
    decimal Price,
    decimal? Discount,
    decimal Rating,
    int Stock,
    Guid CategoryId
);
