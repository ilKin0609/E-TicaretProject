using E_Ticaret_Project.Application.DTOs.SpecialRequestDtos;
using E_Ticaret_Project.Application.Shared.Responses;

namespace E_Ticaret_Project.Application.Abstracts.Services;

public interface ISpecialRequestService
{
    Task<BaseResponse<string>> CreateRequest(SpecialRequestCreateDto dto);
    Task<BaseResponse<string>> RemoveImageAsync(Guid imageId);
}
