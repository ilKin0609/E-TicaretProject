using E_Ticaret_Project.Application.Abstracts.Repositories;
using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.OrderDtos;
using E_Ticaret_Project.Application.DTOs.OrderItemDtos;
using E_Ticaret_Project.Application.Shared;
using E_Ticaret_Project.Application.Shared.Responses;
using E_Ticaret_Project.Domain.Entities;
using E_Ticaret_Project.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace E_Ticaret_Project.Persistence.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEmailService _mailService;
    private readonly UserManager<AppUser> _userManager;

    public OrderService(IOrderRepository orderRepository,
        IProductRepository productRepository,
        IHttpContextAccessor httpContextAccessor,
        IEmailService mailService,
        UserManager<AppUser> userManager)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _httpContextAccessor = httpContextAccessor;
        _mailService = mailService;
        _userManager = userManager;
    }

    public async Task<BaseResponse<string>> CancelOrderAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order is null)
            return new("Order not found", HttpStatusCode.NotFound);

        order.OrderStatus = OrderStatusEnum.Cancelled;
        _orderRepository.Update(order);
        await _orderRepository.SaveChangeAsync();

        return new("Order cancelled",true, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<string>> CreateOrder(OrderCreateDto dto)
    {
        var userId = CurrentUserHelper.GetUserId(_httpContextAccessor.HttpContext);

        var newOrder = new Order
        {
            BuyerId = userId,
            OrderDate = DateTime.UtcNow,
            ShippingAddress = dto.ShippingAddress,
            ShoppingAddress = dto.ShoppingAddress,
            PaymentMethod = dto.PaymentMethod,
            OrderStatus = OrderStatusEnum.Pending,
            TrackingCode = Guid.NewGuid().ToString()[..8]
        };

        decimal totalPrice = 0;
        foreach (var item in dto.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product is null)
                return new("Product not found", HttpStatusCode.NotFound);

            if (product.Stock == 0)
                return new("Product is over", HttpStatusCode.BadRequest);

            if(product.Stock - item.OrderCount<0)
                return new("The requested quantity exceeds the available stock.",HttpStatusCode.BadRequest);

            product.Stock -= item.OrderCount;

            newOrder.Items.Add(new OrderItem
            {
                ProductId = product.Id,
                OrderCount = item.OrderCount,
                FirstPrice = product.Price
            });

            totalPrice += product.Price * item.OrderCount;
        }

        newOrder.TotalPrice = totalPrice;

        await _orderRepository.AddAsync(newOrder);
        await _orderRepository.SaveChangeAsync();

        var link = $"https://localhost:7150/api/Orders/ConfirmOrder?orderId={newOrder.Id}";

        string htmlBody = $@"
            <h2>Order Confirmation</h2>
            <p>Thank you for your order! Please confirm your order by clicking the link below:</p>
            <p><a href='{link}'>Confirm your order</a></p>
            <hr />
            <h4>Order Details:</h4>
            <ul>
                {string.Join("", newOrder.Items.Select(i => $"<li>{i.OrderCount}x {i.Product?.Tittle ?? "Product"} - ${i.FirstPrice}</li>"))}
            </ul>
            <p><strong>Total:</strong> ${newOrder.TotalPrice}</p>
                           ";

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null || string.IsNullOrEmpty(user.Email))
            return new("User not found or email is missing", HttpStatusCode.NotFound);

        await _mailService.SendEmailAsync(
            toEmail: new[] { user.Email },
            subject: "Confirm Your Order",
            body: htmlBody
            );

        return new("Order Created",true, HttpStatusCode.Created);
    }

    public async Task<BaseResponse<string>> UpdateOrderAsync(Guid id,string orderStatus)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order is null)
            return new("Order not found", HttpStatusCode.NotFound);

        order.OrderStatus = (OrderStatusEnum)Enum.Parse(typeof(OrderStatusEnum), orderStatus);
        _orderRepository.Update(order);
        await _orderRepository.SaveChangeAsync();

        return new("Order updated", true, HttpStatusCode.OK);
    }

    
    public async Task<BaseResponse<List<OrderGetDto>>> GetAllAsync()
    {
        var orders = await _orderRepository.GetAllOrderFiltered(
            include: query => query
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
        ).ToListAsync();

        if (!orders.Any())
            return new("No orders found", HttpStatusCode.NotFound);

        var result = new List<OrderGetDto>();

        foreach (var order in orders)
        {
            var validItems = order.Items
                .Where(i => i.Product is not null && !i.Product.IsDeleted)
                .Select(i => new OrderItemGetDto(
                    ProductId: i.ProductId,
                    Tittle: i.Product.Tittle,
                    OrderCount: i.OrderCount,
                    FirstPrice: i.FirstPrice 
                )).ToList();

            var orderDto = new OrderGetDto(
                Id: order.Id,
                OrderStatus: order.OrderStatus,
                OrderDate: order.OrderDate,
                PaymentMethod: order.PaymentMethod,
                TrackingCode: order.TrackingCode,
                ShippingAddress: order.ShippingAddress,
                ShoppingAddress: order.ShoppingAddress,
                TotalPrice: order.TotalPrice,
                BuyerId: order.BuyerId,
                Items: validItems
            );

            result.Add(orderDto);
        }

        return new("All orders", result, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<OrderGetDto>> GetByIdAsync(Guid id)
    {
        var order = await _orderRepository.GetOrderByIdFiltered(
            predicate: o => o.Id == id,
            include: query => query
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
         ).FirstOrDefaultAsync();

        if (order is null)
            return new("Order not found", HttpStatusCode.NotFound);

        var dto = new OrderGetDto(
            Id: id,
            OrderStatus: order.OrderStatus,
            OrderDate: order.OrderDate,
            PaymentMethod: order.PaymentMethod,
            TrackingCode: order.TrackingCode,
            ShippingAddress: order.ShippingAddress,
            ShoppingAddress: order.ShoppingAddress,
            TotalPrice: order.TotalPrice,
            BuyerId: order.BuyerId,

            Items: order.Items.Select(i => new OrderItemGetDto(
                ProductId: i.ProductId,
                Tittle: i.Product.Tittle,
                OrderCount: i.OrderCount,
                FirstPrice: i.Product.Price
                )).ToList()
            );

        return new("Order found", dto, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<List<OrderGetDto>>> GetMyOrdersAsync(string userId)
    {

        var orders = await _orderRepository.GetAllOrderFiltered(
            predicate:o => o.BuyerId == userId,
            include: query => query
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            ).ToListAsync();

        if (!orders.Any())
            return new("You haven't placed any orders.", HttpStatusCode.NotFound);

        var result = new List<OrderGetDto>();
        foreach (var order in orders)
        {
            result.Add(new OrderGetDto(
                Id: order.Id,
                OrderStatus: order.OrderStatus,
                OrderDate: order.OrderDate,
                PaymentMethod: order.PaymentMethod,
                TrackingCode: order.TrackingCode,
                ShippingAddress: order.ShippingAddress,
                ShoppingAddress: order.ShoppingAddress,
                TotalPrice: order.TotalPrice,
                BuyerId: userId,

                Items: order.Items.Select(i => new OrderItemGetDto(
                    ProductId: i.ProductId,
                    Tittle: i.Product?.Tittle ?? "Unknown",
                    OrderCount: i.OrderCount,
                    FirstPrice: i.Product?.Price ?? 0
                    )).ToList()
                ));
        }
        return new("All your orders", result, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<List<OrderGetDto>>> GetMySalesAsync()
    {
        var sellerId = CurrentUserHelper.GetUserId(_httpContextAccessor.HttpContext);

        var orders = await _orderRepository.GetAllOrderFiltered(
            predicate: o => o.Items.Any(i => i.Product.OwnerId == sellerId),
            include: query => query
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            ).ToListAsync();

        var result = new List<OrderGetDto>();

        foreach (var order in orders)
        {
            var orderItems = order.Items
                .Where(i => i.Product.OwnerId == sellerId)
                .Select(i => new OrderItemGetDto(
                    ProductId: i.ProductId,
                    Tittle: i.Product.Tittle,
                    OrderCount: i.OrderCount,
                    FirstPrice: i.FirstPrice
                )).ToList();

            var dto = new OrderGetDto(
                Id: order.Id,
                OrderStatus: order.OrderStatus,
                OrderDate: order.OrderDate,
                PaymentMethod: order.PaymentMethod,
                TrackingCode: order.TrackingCode,
                ShippingAddress: order.ShippingAddress,
                ShoppingAddress: order.ShoppingAddress,
                TotalPrice: order.TotalPrice,
                BuyerId: order.BuyerId,
                Items: orderItems
            );

            result.Add(dto);
        }

        return new("All your product sales", result, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<string>> ConfirmOrder(Guid orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order is null)
            return new("Order not found", HttpStatusCode.NotFound);

        order.OrderStatus = OrderStatusEnum.Confirmed;
        _orderRepository.Update(order);
        await _orderRepository.SaveChangeAsync();

        return new("Order confirmed successfully",true, HttpStatusCode.OK);
    }

}
