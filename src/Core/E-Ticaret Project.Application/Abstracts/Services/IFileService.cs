using Microsoft.AspNetCore.Http;

namespace E_Ticaret_Project.Application.Abstracts.Services;

public interface IFileService
{
    Task<string> UploadAsync(IFormFile file);
}
