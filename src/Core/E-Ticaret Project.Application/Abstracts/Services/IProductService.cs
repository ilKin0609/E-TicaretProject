
using E_Ticaret_Project.Application.DTOs.ProductDtos;
using E_Ticaret_Project.Application.Shared.Responses;
using Microsoft.AspNetCore.Http;

namespace E_Ticaret_Project.Application.Abstracts.Services;

public interface IProductService
{
    // CRUD
    Task<BaseResponse<string>> CreateAsync(ProductCreateDto dto);
    Task<BaseResponse<string>> UpdateAsync(ProductUpdateDto dto);
    Task<BaseResponse<string>> DeleteAsync(Guid id);

    //// DETAIL (kartdan keçid üçün)
    Task<BaseResponse<ProductDetailDto>> GetDetailByIdAsync(Guid id);
    Task<BaseResponse<ProductDetailDto>> GetDetailBySidAsync(string sid); // URL-lik qısa ID

    //// LIST (kartlar) 
    Task<BaseResponse<List<ProductCardDto>>> GetByCategoryAsync(Guid categoryId);
    Task<BaseResponse<List<ProductCardDto>>> GetByTagId(Guid tagId);
    Task<BaseResponse<List<ProductCardDto>>> GetByTagAsync(string tag);
    Task<BaseResponse<List<ProductCardDto>>> GetByTagsAsync(IEnumerable<string> tags);
    Task<BaseResponse<List<ProductCardDto>>> SearchAsync(string q); // title+desc
    Task<BaseResponse<ProductCardDto>> GetBySKUAsync(string sku); // dəqiq uyğunluq (tez tapmaq üçün)
    Task<BaseResponse<List<ProductCardDto>>> GetAllAsync(int size = 40);

    //// ŞƏKİLLƏR
    Task<BaseResponse<string>> UploadMainImageAsync(ProductMainImageUploadDto dto);
    Task<BaseResponse<string>> UploadAdditionalImageAsync(ProductAdditionalImageUploadDto dto);
    Task<BaseResponse<string>> RemoveImageAsync(Guid imageId);
    Task<BaseResponse<string>> SetMainImageAsync(SetMainDto dto);                  // tövsiyə olunur
    Task<BaseResponse<string>> ReorderImagesAsync(ListedReorderDto dto);  // tövsiyə olunur
    Task<BaseResponse<string>> UpdateImageAltAsync(ProductUpdateAltDto dto);

    Task<BaseResponse<List<ProductImageDto>>> GetImagesAsync(Guid productId);
    Task<BaseResponse<ProductImageDto>> GetMainImageAsync(Guid productId);

}
