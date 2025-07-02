namespace E_Ticaret_Project.Domain.Entities;

public class Favorite:BaseEntity
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; }

    public string UserId { get; set; }
    public AppUser User { get; set; }
}
