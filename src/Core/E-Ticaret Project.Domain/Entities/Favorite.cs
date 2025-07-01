namespace E_Ticaret_Project.Domain.Entities;

public class Favorite:BaseEntity
{
    public ICollection<Product> Products { get; set; }
}
