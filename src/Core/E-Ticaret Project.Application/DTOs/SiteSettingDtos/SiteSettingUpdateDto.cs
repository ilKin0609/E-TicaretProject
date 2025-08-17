namespace E_Ticaret_Project.Application.DTOs.SiteSettingDtos;

public record SiteSettingUpdateDto(
    bool HideProductPrices,
    bool DisablePartnerLogin,
    bool HidePopCategory,
    bool HideSearchBar,
    string? ScrollText,
    int? ScrollTextSpeed,
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
