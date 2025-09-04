namespace E_Ticaret_Project.Domain.Entities;

public class ProductImage:BaseEntity
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public string Url { get; set; } = null!;           // Cloudinary secure_url
    public string PublicId { get; set; } = null!;      // Cloudinary public_id
    public bool IsMain { get; set; }                   // yalnız 1 ədəd true
    public int SortOrder { get; set; }                 // qalereyada düzülüş

    public string? AltAz { get; set; }
    public string? AltEn { get; set; }
    public string? AltRu { get; set; }
}
