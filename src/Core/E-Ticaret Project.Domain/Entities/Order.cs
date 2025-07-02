namespace E_Ticaret_Project.Domain.Entities;

public class Order:BaseEntity
{
    public string OrderStatus { get; set; }
    public DateTime OrderDate { get; set; }
    public string PaymentMethod { get; set; }
    public string TrackingCode { get; set; }
    public string? ShippingAddress { get; set; }
    public string ShoppingAddress { get; set; }


    public string BuyerId { get; set; }
    public AppUser Buyer { get; set; }

    public ICollection<OrderItem> Items { get; set; }=new List<OrderItem>();
}
