using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace E_Ticaret_Project.Persistence.Configurations;

public class SiteSettingConfiguration : IEntityTypeConfiguration<SiteSetting>
{
    public void Configure(EntityTypeBuilder<SiteSetting> builder)
    {
        builder.HasData(new SiteSetting
        {
            Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), // sabit Id
            HideProductPrices = false,
            DisablePartnerLogin = false,
            HidePopCategory = false,
            HideSearchBar = false,
            ScrollText = "Xoş gəlmisiniz!",
            WhatsappInquiryLink = "https://wa.me/12025550123",
            InstagramUrl = "https://www.facebook.com/NASA",
            FacebookUrl = "https://www.instagram.com/instagram",
            YoutubeUrl = "https://www.youtube.com/watch?v=jfKfPfyJRdk",
            CreatedAt = DateTime.UtcNow,
            CreatedUser = null,
            IsDeleted = false
        });

        builder.Property(x => x.ScrollText).HasMaxLength(1000);
        builder.Property(x => x.WhatsappInquiryLink).HasMaxLength(500);
    }
}

