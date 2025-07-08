namespace E_Ticaret_Project.Application.DTOs.OrderItemDtos;

public record OrderItemGetDto(

    Guid ProductId,
    string Tittle,
    int OrderCount,
    decimal FirstPrice
);
