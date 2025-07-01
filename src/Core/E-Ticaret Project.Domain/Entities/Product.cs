namespace E_Ticaret_Project.Domain.Entities;

public class Product:BaseEntity
{
    public string Tittle { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }

    public Guid CategoryId { get; set; }
    public Category Category { get; set; }

    public Guid OwnerId { get; set; }
    public AppUser Owner { get; set; }

    public Guid FavoriteId { get; set; }
    public Favorite Favorite { get; set; }

    public ICollection<OrderItem> Items { get; set; }=new List<OrderItem>();
    public ICollection<Image> Images { get; set; }=new List<Image>();
    public ICollection<ReviewAndComment> Comments { get; set; }
}
