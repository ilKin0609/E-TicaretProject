namespace E_Ticaret_Project.Domain.Entities;

public class Image:BaseEntity
{
    public string Image_Url { get; set; }
    public bool is_main { get; set; }
    public string? PublicId { get; set; }

    public Guid ProductId { get; set; }
    public Product Product { get; set; }
}
