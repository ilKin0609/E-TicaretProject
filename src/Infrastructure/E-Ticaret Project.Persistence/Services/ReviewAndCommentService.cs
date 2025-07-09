using E_Ticaret_Project.Application.Abstracts.Repositories;
using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.ReviewAndCommentDtos;
using E_Ticaret_Project.Application.Shared.Responses;
using E_Ticaret_Project.Domain.Entities;
using E_Ticaret_Project.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace E_Ticaret_Project.Persistence.Services;

public class ReviewAndCommentService : IReviewAndCommentService
{
    private IReviewAndCommentRepository _commentRepository { get; }
    private IProductRepository _productRepository { get; }

    public ReviewAndCommentService(IReviewAndCommentRepository commentRepository,IProductRepository productRepository)
    {
        _commentRepository = commentRepository;
        _productRepository = productRepository;
    }

    public async Task<BaseResponse<string>> AddComment(ReviewAndCommentCreateDto dto)
    {
        var comment = new ReviewAndComment()
        {
            Comment = dto.Comment,
            UserId = dto.UserId,
            ProductId = dto.ProductId
        };

        await _commentRepository.AddAsync(comment);
        await _commentRepository.SaveChangeAsync();

        return new("Comment added",true, HttpStatusCode.Created);
    }

    public async Task<BaseResponse<string>> RemoveComment(Guid id)
    {
        var comment = await _commentRepository.GetByIdAsync(id);
        if (comment is null)
            return new("Comment is not found", HttpStatusCode.NotFound);

        _commentRepository.Delete(comment);
        await _commentRepository.SaveChangeAsync();

        return new("Comment is deleted",true, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<List<ReviewAndCommentGetDto>>> GetByProductIdAsync(Guid productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product is null)
            return new("Product is not found", HttpStatusCode.NotFound);

        var comments = await _commentRepository
       .GetAllFiltered(
           predicate: rc => rc.ProductId == productId,
           include: [
               rc => rc.User
           ])
       .ToListAsync();

        if (!comments.Any())
            return new("No comments found for this product.", HttpStatusCode.NotFound);

        var commentDtos = comments.Select(c =>new ReviewAndCommentGetDto(
            c.Id, 
            c.Comment, 
            c.ProductId, 
            c.UserId)
        ).ToList();

        return new("Comments", commentDtos,HttpStatusCode.OK);
    }
}
