using E_Ticaret_Project.Application.Abstracts.Repositories;
using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.ReviewAndCommentDtos;
using E_Ticaret_Project.Application.Shared.Responses;

namespace E_Ticaret_Project.Persistence.Services;

public class ReviewAndCommentService:IReviewAndCommentService
{
    private IReviewAndCommentRepository _commentRepository { get; }

    public ReviewAndCommentService(IReviewAndCommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public Task<BaseResponse<string>> AddComment(ReviewAndCommentCreateDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<List<ReviewAndCommentGetDto>>> GetMyComments(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<string>> RemoveComment(Guid id)
    {
        throw new NotImplementedException();
    }
}
