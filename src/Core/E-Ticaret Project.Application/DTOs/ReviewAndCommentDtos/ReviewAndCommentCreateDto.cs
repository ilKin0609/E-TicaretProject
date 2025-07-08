namespace E_Ticaret_Project.Application.DTOs.ReviewAndCommentDtos;

public record ReviewAndCommentCreateDto(

    string Comment,
    Guid ProductId,
    string UserId
);
