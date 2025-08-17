using E_Ticaret_Project.Domain.Entities;

public class AboutUs : BaseEntity
{
    public string MetaTitle_Az { get; set; }
    public string MetaTitle_En { get; set; }
    public string MetaTitle_Ru { get; set; }

    public string MetaDescription_Az { get; set; }
    public string MetaDescription_En { get; set; }
    public string MetaDescription_Ru { get; set; }

    public string Keywords { get; set; }

    public string TitleAZ { get; set; }
    public string TitleEN { get; set; }
    public string TitleRU { get; set; }

    public string DescriptionAZ { get; set; }
    public string DescriptionEN { get; set; }
    public string DescriptionRU { get; set; }

    public Image? Image { get; set; }
}
