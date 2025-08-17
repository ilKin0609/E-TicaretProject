namespace E_Ticaret_Project.Domain.Entities;

public class Product:BaseEntity
{
    public string SID { get; set; } = null!;            // URL üçün qısa id
    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; } = null!;

    public string SKU { get; set; } = null!;            // admin verir, mehsul kodu
    public decimal? PriceAZN { get; set; }
    public decimal? PartnerPriceAZN { get; set; }


    // Çoxdilli başlıq və mətn
    public string TitleAz { get; set; } = null!;
    public string? TitleEn { get; set; }
    public string? TitleRu { get; set; }

    public string? DescAz { get; set; }               
    public string? DescEn { get; set; }
    public string? DescRu { get; set; }

    // Meta
    public string? MetaTitleAz { get; set; }
    public string? MetaTitleEn { get; set; }
    public string? MetaTitleRu { get; set; }

    public string? MetaDescriptionAz { get; set; }
    public string? MetaDescriptionEn { get; set; }
    public string? MetaDescriptionRu { get; set; }

    // YALNIZ AZ slugu saxlayırıq
    public string? SlugAz { get; set; }

    public ICollection<ProductTag> ProductTags { get; set; }
    
    public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
}
