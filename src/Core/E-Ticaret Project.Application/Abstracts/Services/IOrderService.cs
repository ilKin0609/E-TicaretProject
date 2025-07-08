using E_Ticaret_Project.Application.DTOs.OrderDtos;
using E_Ticaret_Project.Application.Shared.Responses;

namespace E_Ticaret_Project.Application.Abstracts.Services;

public interface IOrderService
{
    Task<BaseResponse<string>> CreateOrder(OrderCreateDto dto);
    Task<BaseResponse<string>> CancelOrderAsync(Guid id);
    Task<BaseResponse<string>> UpdateOrderAsync(Guid id, string orderStatus);
    Task<BaseResponse<OrderGetDto>> GetByIdAsync(Guid id);
    Task<BaseResponse<List<OrderGetDto>>> GetMyOrdersAsync(string userId);
    Task<BaseResponse<List<OrderGetDto>>> GetAllAsync();
    Task<BaseResponse<List<OrderGetDto>>> GetMySalesAsync();

    Task<BaseResponse<string>> ConfirmOrder(Guid orderId);
}
