using E_Ticaret_Project.Application.DTOs.CategoryDtos;
using E_Ticaret_Project.Application.Shared.Responses;

namespace E_Ticaret_Project.Application.Abstracts.Services;

public interface ICategoryService
{
    Task<BaseResponse<string>> CreateAsync(CategoryCreateDto dto);
    Task<BaseResponse<string>> UpdateAsync(CategoryUpdateDto dto);
    Task<BaseResponse<string>> DeleteAsync(Guid id);
    Task<BaseResponse<CategoryGetDto>> GetByIdAsync(Guid id);
    Task<BaseResponse<List<CategoryGetDto>>> GetByNameAsync(string name);
    Task<BaseResponse<List<CategoryGetDto>>> GetAllRecursiveAsync();
}
