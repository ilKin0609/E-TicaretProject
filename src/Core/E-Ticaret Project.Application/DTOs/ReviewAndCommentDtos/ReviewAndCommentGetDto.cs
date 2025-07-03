namespace E_Ticaret_Project.Application.DTOs.ReviewAndCommentDtos;

public record ReviewAndCommentGetDto(

    Guid Id,
    string Comment,
    Guid ProductId,
    string UserId
);
