using E_Ticaret_Project.Application.DTOs.ProductDtos;
using E_Ticaret_Project.Application.Shared.Responses;

namespace E_Ticaret_Project.Application.Abstracts.Services;

public interface IProductService
{
    Task<BaseResponse<string>> CreateProduct(ProductCreateDto dto);
    Task<BaseResponse<ProductUpdateDto>> UpdateProduct(ProductUpdateDto dto);
    Task<BaseResponse<string>> DeleteProduct(Guid productId);

    Task<BaseResponse<List<ProductGetDto>>> GetAllProduct();
    Task<BaseResponse<ProductGetDto>> GetByIdProduct(Guid productId);
    Task<BaseResponse<List<ProductGetDto>>> GetByTitleProduct(string Title);
    Task<BaseResponse<List<ProductGetDto>>> GetMyProducts(string userId);
    Task<BaseResponse<List<ProductGetDto>>> GetByCategoryProducts(Guid categoryId);
    Task<BaseResponse<List<ProductGetDto>>> GetDiscountProducts();
}
