using Microsoft.AspNetCore.Http;

namespace E_Ticaret_Project.Application.Abstracts.Services;

public interface ICloudinaryService
{
    Task<(string ImageUrl, string PublicId)> UploadImageAsync(IFormFile file, string folderName);
    Task<bool> DeleteImageAsync(string publicId);
}
