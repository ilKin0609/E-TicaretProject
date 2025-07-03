using E_Ticaret_Project.Application.DTOs.OrderItemDtos;
using E_Ticaret_Project.Domain.Enums;

namespace E_Ticaret_Project.Application.DTOs.OrderDtos;

public record OrderCreateDto(

    string ShippingAddress,
    string ShoppingAddress,
    PaymentMethodEnum PaymentMethod,
    List<OrderItemCreateDto> Items
);
