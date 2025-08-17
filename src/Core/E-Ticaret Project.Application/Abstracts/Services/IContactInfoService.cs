using E_Ticaret_Project.Application.DTOs.ContactInfoDtos;
using E_Ticaret_Project.Application.Shared.Responses;

namespace E_Ticaret_Project.Application.Abstracts.Services;

public interface IContactInfoService
{
    Task<BaseResponse<string>> UpdateAsync(ContactInfoUpdateDto dto);
    Task<BaseResponse<ContactInfoGetDto>> GetContact();
}
