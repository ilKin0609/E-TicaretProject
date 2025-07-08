using Microsoft.AspNetCore.Http;

namespace E_Ticaret_Project.Application.DTOs.ProductDtos;

public record ProductCreateDto(
    string Tittle,
    string? Description,
    decimal Price,
    int? Discount,
    decimal? Rating,
    int Stock,
    List<IFormFile>? image,
    Guid CategoryId
);
