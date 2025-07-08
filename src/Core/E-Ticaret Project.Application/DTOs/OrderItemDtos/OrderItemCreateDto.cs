namespace E_Ticaret_Project.Application.DTOs.OrderItemDtos;

public record OrderItemCreateDto(

    Guid ProductId,
    int OrderCount
);
