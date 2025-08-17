namespace E_Ticaret_Project.Domain.Entities;

public class ContactInfo:BaseEntity
{
    public string MetaTitle_Az { get; set; }
    public string MetaTitle_En { get; set; }
    public string MetaTitle_Ru { get; set; }

    public string MetaDescription_Az { get; set; }
    public string MetaDescription_En { get; set; }
    public string MetaDescription_Ru { get; set; }

    public string Keywords { get; set; }

    public string Title_Az { get; set; }
    public string Title_En { get; set; }
    public string Title_Ru { get; set; }

    public string Phone { get; set; }
    public string Email { get; set; }

    public string AddressAZ { get; set; }
    public string AddressEN { get; set; }
    public string AddressRU { get; set; }

    public string MapIframeSrc { get; set; }
}
