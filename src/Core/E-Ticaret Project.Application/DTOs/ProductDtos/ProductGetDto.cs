
using E_Ticaret_Project.Application.DTOs.ReviewAndCommentDtos;

namespace E_Ticaret_Project.Application.DTOs.ProductDtos;

public record ProductGetDto(

    Guid Id,
    string Title,
    string? Description,
    decimal Price,
    decimal? Discount,
    decimal Rating,
    int Stock,
    Guid CategoryId,
    string OwnerId,
    List<string>? ImageUrls

);
