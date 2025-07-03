using E_Ticaret_Project.Domain.Enums;

namespace E_Ticaret_Project.Domain.Entities;

public class Order:BaseEntity
{
    public OrderStatusEnum OrderStatus { get; set; }
    public PaymentMethodEnum PaymentMethod { get; set; }
    public DateTime OrderDate { get; set; }
    public string TrackingCode { get; set; }
    public string ShippingAddress { get; set; } = null!;
    public string ShoppingAddress { get; set; }
    public decimal TotalPrice { get; set; }


    public string BuyerId { get; set; }
    public AppUser Buyer { get; set; }

    public ICollection<OrderItem> Items { get; set; }=new List<OrderItem>();
}
