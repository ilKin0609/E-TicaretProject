using Microsoft.AspNetCore.Http;

namespace E_Ticaret_Project.Application.DTOs.ProductDtos;

public record ProductMainImageUploadDto
(
    Guid ProductId,
    IFormFile File
);
