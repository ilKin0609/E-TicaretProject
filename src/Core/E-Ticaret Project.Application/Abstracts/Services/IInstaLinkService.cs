using E_Ticaret_Project.Application.DTOs.InstaLinkDtos;
using E_Ticaret_Project.Application.Shared.Responses;

namespace E_Ticaret_Project.Application.Abstracts.Services;

public interface IInstaLinkService
{
    Task<BaseResponse<InstagramLinkVm>> AddAsync(InstaLinkCreateDto dto);
    Task<BaseResponse<string>> DeleteAsync(InstagramLinkDeleteDto dto);
    Task<IReadOnlyList<InstagramLinkVm>> GetRandomAsync(int count = 6);
}
