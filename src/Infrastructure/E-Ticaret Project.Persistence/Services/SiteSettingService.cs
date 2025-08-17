using E_Ticaret_Project.Application.Abstracts.Repositories;
using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.SiteSettingDtos;
using E_Ticaret_Project.Application.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace E_Ticaret_Project.Persistence.Services;

public class SiteSettingService:ISiteSettingService
{
    private ISiteSettingRepository _siteRepo { get; }
    private IContactInfoRepository _contactRepo { get; }

    public SiteSettingService(
        ISiteSettingRepository siteRepo,
        IContactInfoRepository contactRepo)
    {
        _siteRepo = siteRepo;
        _contactRepo = contactRepo;
    }

    public async Task<BaseResponse<SiteSettingGetDto>> GetAsync()
    {
        var s = await _siteRepo.GetAll().AsNoTracking().FirstOrDefaultAsync();
        if (s is null)
            return new("Ayarlar tapılmadı.", HttpStatusCode.NotFound);

        return new("Ayarlar gətirildi.", MapToGetDto(s), HttpStatusCode.OK);
    }

    public async Task<BaseResponse<string>> UpdateAsync(SiteSettingUpdateDto dto)
    {
        // keywords-ları təmizləyən helper
        string? Clean(string? src)
        {
            if (string.IsNullOrWhiteSpace(src)) return null;
            var parts = src.Split(',', StringSplitOptions.RemoveEmptyEntries)
                           .Select(p => p.Trim())
                           .Where(p => p.Length > 0)
                           .Distinct(StringComparer.OrdinalIgnoreCase);
            return string.Join(", ", parts);
        }

        var current = await _siteRepo.GetAll(isTracking: true).FirstOrDefaultAsync();
        var oldPublicEmail = current?.PublicEmail;

        if (current is null)
        {
            current = new SiteSetting();
            await _siteRepo.AddAsync(current); // upsert: yoxdursa yarad
        }

        // dto -> entity
        current.HideProductPrices = dto.HideProductPrices;
        current.DisablePartnerLogin = dto.DisablePartnerLogin;
        current.HidePopCategory = dto.HidePopCategory;
        current.HideSearchBar = dto.HideSearchBar;

        current.ScrollText = dto.ScrollText;
        current.ScrollTextSpeed = dto.ScrollTextSpeed;
        current.WhatsappInquiryLink = dto.WhatsappInquiryLink;

        current.PublicEmail = dto.PublicEmail;
        current.FacebookUrl = dto.FacebookUrl;
        current.InstagramUrl = dto.InstagramUrl;
        current.YoutubeUrl = dto.YouTubeUrl;

        current.HomeMetaTitleAz = dto.HomeMetaTitleAz;
        current.HomeMetaTitleRu = dto.HomeMetaTitleRu;
        current.HomeMetaTitleEn = dto.HomeMetaTitleEn;

        current.HomeMetaDescriptionAz = dto.HomeMetaDescriptionAz;
        current.HomeMetaDescriptionRu = dto.HomeMetaDescriptionRu;
        current.HomeMetaDescriptionEn = dto.HomeMetaDescriptionEn;

        current.HomeKeywords = Clean(dto.HomeKeywords);
        

        current.GoogleSiteVerification = dto.GoogleSiteVerification;
        current.YandexSiteVerification = dto.YandexVerification;
        current.BingSiteVerification = dto.BingVerification;

        _siteRepo.Update(current);
        await _siteRepo.SaveChangeAsync();

        // PublicEmail dəyişibsə, ContactInfo.Email də sinxron olsun
        if (!string.Equals(oldPublicEmail, current.PublicEmail, StringComparison.OrdinalIgnoreCase))
        {
            var ci = await _contactRepo.GetAll(isTracking: true).FirstOrDefaultAsync();
            if (ci != null && !string.Equals(ci.Email, current.PublicEmail, StringComparison.OrdinalIgnoreCase))
            {
                ci.Email = current.PublicEmail;
                _contactRepo.Update(ci);
                await _contactRepo.SaveChangeAsync();
            }
        }

        return new("Ayarlar yeniləndi.", true, HttpStatusCode.OK);
    }




    private static SiteSettingGetDto MapToGetDto(SiteSetting s) => new(
        s.HideProductPrices,
        s.DisablePartnerLogin,
        s.HidePopCategory,
        s.HideSearchBar,
        s.ScrollText,
        s.ScrollTextSpeed,
        s.WhatsappInquiryLink,
        s.PublicEmail,
        s.FacebookUrl,
        s.InstagramUrl,
        s.YoutubeUrl,
        s.HomeMetaTitleAz,
        s.HomeMetaTitleRu,
        s.HomeMetaTitleEn,
        s.HomeMetaDescriptionAz,
        s.HomeMetaDescriptionRu,
        s.HomeMetaDescriptionEn,
        s.HomeKeywords,
        s.GoogleSiteVerification,
        s.YandexSiteVerification,
        s.BingSiteVerification
    );
}
