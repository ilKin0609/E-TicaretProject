namespace E_Ticaret_Project.Domain.Entities;

public class Image:BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string ImageUrl { get; set; }
    public bool IsMain { get; set; }
    public string? PublicId { get; set; }

    public Guid? AboutUsId { get; set; }
    public AboutUs? AboutUs { get; set; }

    public Guid? SpecialRequestId { get; set; }
    public SpecialRequest? SpecialRequest { get; set; }
}
