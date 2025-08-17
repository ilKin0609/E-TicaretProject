using E_Ticaret_Project.Application.DTOs.AboutUsDtos;
using E_Ticaret_Project.Application.Shared.Responses;

namespace E_Ticaret_Project.Application.Abstracts.Services;

public interface IAboutService
{
    Task<BaseResponse<string>> UpdateAsync(AboutUsUpdateDto dto);
    Task<BaseResponse<string>> AboutUploadImage(AboutUsUploadImageDto dto);
    Task<BaseResponse<string>> RemoveImageAsync(Guid imageId);
    Task<BaseResponse<AboutUsGetDto>> GetAbout();
}
