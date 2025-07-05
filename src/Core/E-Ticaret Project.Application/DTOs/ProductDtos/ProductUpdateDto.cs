using Microsoft.AspNetCore.Http;

namespace E_Ticaret_Project.Application.DTOs.ProductDtos;

public record ProductUpdateDto(

    Guid Id,
    string Title,
    string? Description,
    decimal Price,
    decimal? Discount,
    decimal? Rating,
    int? Stock,
    Guid? CategoryId
);

