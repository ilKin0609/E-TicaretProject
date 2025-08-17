namespace E_Ticaret_Project.Domain.Entities;

public class Category : BaseEntity
{
    public string NameAz { get; set; }
    public string NameRu { get; set; } 
    public string NameEn { get; set; }

    public string? Slug { get; set; }

    public Guid? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
    public ICollection<Category> SubCategories { get; set; }

    public string? MetaTitleAz { get; set; }
    public string? MetaTitleRu { get; set; }
    public string? MetaTitleEn { get; set; }

    public string? MetaDescriptionAz { get; set; }
    public string? MetaDescriptionRu { get; set; }
    public string? MetaDescriptionEn { get; set; }

    public string? Keywords { get; set; }

    public bool IsVisible { get; set; } = true;
    public int Order { get; set; } = 0;
    public ICollection<Product> Products {get; set;}=new List<Product>();

}
