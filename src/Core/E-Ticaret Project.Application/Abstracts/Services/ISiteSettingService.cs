using E_Ticaret_Project.Application.DTOs.SiteSettingDtos;
using E_Ticaret_Project.Application.Shared.Responses;

namespace E_Ticaret_Project.Application.Abstracts.Services;

public interface ISiteSettingService
{
    Task<BaseResponse<SiteSettingGetDto>> GetAsync();
    Task<BaseResponse<string>> UpdateAsync(SiteSettingUpdateDto dto); // UPSERT
}
