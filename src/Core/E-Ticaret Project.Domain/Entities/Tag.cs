namespace E_Ticaret_Project.Domain.Entities;

public class Tag:BaseEntity
{
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;

    public ICollection<ProductTag> ProductTags { get; set; }
}
