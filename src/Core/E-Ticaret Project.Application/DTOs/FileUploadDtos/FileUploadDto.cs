using Microsoft.AspNetCore.Http;

namespace E_Ticaret_Project.Application.DTOs.FileUploadDtos;

public record FileUploadDto
{
    public IFormFile File { get; set; }
}
