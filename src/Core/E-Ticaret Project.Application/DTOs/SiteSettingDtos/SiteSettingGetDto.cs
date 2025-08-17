namespace E_Ticaret_Project.Application.DTOs.SiteSettingDtos;

public record SiteSettingGetDto(
    bool HideProductPrices,
    bool DisablePartnerLogin,
    bool HidePopCategory,
    bool HideSearchBar,
    string? ScrollText,
    int? ScroolTextSpeed,
    string? WhatsappInquiryLink,
    string? PublicEmail,
    string? FacebookUrl,
    string? InstagramUrl,
    string? YouTubeUrl,
    string? HomeMetaTitleAz,
    string? HomeMetaTitleRu,
    string? HomeMetaTitleEn,
    string? HomeMetaDescriptionAz,
    string? HomeMetaDescriptionRu,
    string? HomeMetaDescriptionEn,
    string? HomeKeywords,
    string? GoogleSiteVerification,
    string? YandexVerification,
    string? BingVerification
);
