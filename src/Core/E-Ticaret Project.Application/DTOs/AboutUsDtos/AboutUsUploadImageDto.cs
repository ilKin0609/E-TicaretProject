using Microsoft.AspNetCore.Http;

namespace E_Ticaret_Project.Application.DTOs.AboutUsDtos;

public record class AboutUsUploadImageDto(IFormFile ImageFile);
