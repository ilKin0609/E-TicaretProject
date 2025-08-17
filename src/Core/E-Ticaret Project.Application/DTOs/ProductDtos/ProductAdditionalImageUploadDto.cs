using Microsoft.AspNetCore.Http;

namespace E_Ticaret_Project.Application.DTOs.ProductDtos;

public record ProductAdditionalImageUploadDto
(
    Guid ProductId,
    IFormFile File
);
