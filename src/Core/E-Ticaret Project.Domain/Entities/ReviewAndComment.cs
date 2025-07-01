namespace E_Ticaret_Project.Domain.Entities;

public class ReviewAndComment:BaseEntity
{
    public string Comment { get; set; }


    public string UserId { get; set; }
    public AppUser User { get; set; }

    public Guid ProductId { get; set; }
    public Product Product { get; set; }

}
