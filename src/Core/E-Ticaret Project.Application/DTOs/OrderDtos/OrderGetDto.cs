using E_Ticaret_Project.Application.DTOs.OrderItemDtos;
using E_Ticaret_Project.Domain.Enums;

namespace E_Ticaret_Project.Application.DTOs.OrderDtos;

public record OrderGetDto(

    Guid Id,
    OrderStatusEnum OrderStatus,
    DateTime OrderDate,
    PaymentMethodEnum PaymentMethod,
    string TrackingCode,
    string ShippingAddress,
    string ShoppingAddress,
    decimal TotalPrice,
    string BuyerId,
    ICollection<OrderItemGetDto> Items
);
