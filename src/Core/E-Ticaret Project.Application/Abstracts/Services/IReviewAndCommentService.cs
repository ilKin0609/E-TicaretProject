using E_Ticaret_Project.Application.DTOs.ReviewAndCommentDtos;
using E_Ticaret_Project.Application.Shared.Responses;

namespace E_Ticaret_Project.Application.Abstracts.Services;

public interface IReviewAndCommentService
{
    Task<BaseResponse<string>> AddComment(ReviewAndCommentCreateDto dto);
    Task<BaseResponse<List<ReviewAndCommentGetDto>>> GetByProductIdAsync(Guid productId);
    Task<BaseResponse<string>> RemoveComment(Guid id);
}
