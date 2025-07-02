namespace E_Ticaret_Project.Domain.Entities;

public class Product:BaseEntity
{
    public string Tittle { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public decimal? Discount { get; set; }
    public decimal Rating { get; set; }
    public int Stock { get; set; }

    public Guid CategoryId { get; set; }
    public Category Category { get; set; }

    public string OwnerId { get; set; }
    public AppUser Owner { get; set; }



    public ICollection<OrderItem> Items { get; set; }=new List<OrderItem>();
    public ICollection<Image> Images { get; set; }=new List<Image>();
    public ICollection<ReviewAndComment> Comments { get; set; }
    public ICollection<Favorite> Favorites { get; set; }
}
