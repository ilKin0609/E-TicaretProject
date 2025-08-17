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
    Task<BaseResponse<CategoryGetDto>> GetBySlugAsync(string slug);
    Task<BaseResponse<List<CategoryGetDto>>> GetAllMainCategoriesAsync();
    Task<BaseResponse<List<CategoryGetDto>>> GetAllSubCategoriesAsync(Guid parentId);
    Task<BaseResponse<List<CategoryBreadcrumbItemDto>>> GetBreadcrumbAsync(Guid id);
    Task<BaseResponse<List<PopularTagDto>>> GetPopularTagsFromSearchAsync(int take = 8);
    Task<BaseResponse<List<CategoryGetDto>>> GetAllRecursiveAsync();

    Task<BaseResponse<string>> ToggleVisibilityAsync(Guid id, bool isVisible);
    Task<BaseResponse<string>> ReorderAsync(CategoryReorderDto dto);
    Task<BaseResponse<string>> ReorderBulkAsync(List<CategoryReorderDto> items);
    Task<BaseResponse<string>> ChangeParentAsync(Guid id, Guid? newParentId);
    Task<BaseResponse<bool>> IsLeafAsync(Guid id);
    Task<BaseResponse<string>> GenerateSlugAsync(string nameAz);
    Task<BaseResponse<string>> IncrementKeywordSearchAsync(string rawKeyword, long delta = 1);
    Task IncrementKeywordSearchManyAsync(IEnumerable<string> rawKeywords);

}
