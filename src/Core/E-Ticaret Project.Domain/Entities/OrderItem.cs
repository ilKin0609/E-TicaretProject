namespace E_Ticaret_Project.Domain.Entities;

public class OrderItem
{
    public int OrderCount { get; set; }
    public decimal FirstPrice { get; set; }
    public decimal TotalPrice => OrderCount * FirstPrice;


    public Guid OrderId { get; set; }
    public Order Order { get; set; }

    public Guid ProductId { get; set; }
    public Product Product { get; set; }

}
