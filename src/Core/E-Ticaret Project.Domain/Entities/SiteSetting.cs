using E_Ticaret_Project.Domain.Entities;

public class SiteSetting : BaseEntity
{
    public bool HideProductPrices { get; set; } = false;
    public bool DisablePartnerLogin { get; set; } = false;
    public bool HidePopCategory { get; set; } = false;
    public bool HideSearchBar { get; set; } = false;

    public string? ScrollText { get; set; }
    public int? ScrollTextSpeed { get; set; }
    public string? WhatsappInquiryLink { get; set; }

    public string? FacebookUrl { get; set; }
    public string? InstagramUrl { get; set; }
    public string? YoutubeUrl { get; set; }

    public string? PublicEmail { get; set; }

    public string? HomeMetaTitleAz { get; set; }
    public string? HomeMetaTitleEn { get; set; }
    public string? HomeMetaTitleRu { get; set; }

    public string? HomeMetaDescriptionAz { get; set; }
    public string? HomeMetaDescriptionEn { get; set; }
    public string? HomeMetaDescriptionRu { get; set; }

    public string? HomeKeywords { get; set; }

    // Axtarış sistemləri / izləmə kodları
    public string? GoogleSiteVerification { get; set; }  // meta və ya tam snippet
    public string? BingSiteVerification { get; set; }
    public string? YandexSiteVerification { get; set; }

  
}

