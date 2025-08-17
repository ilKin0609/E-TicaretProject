using E_Ticaret_Project.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Ticaret_Project.Persistence.Configurations;

public class AboutConfiguration : IEntityTypeConfiguration<AboutUs>
{
    public void Configure(EntityTypeBuilder<AboutUs> builder)
    {
        builder.HasData(new AboutUs
        {
            Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),

            MetaTitle_Az = "LABstend - Reklam Xidməti",
            MetaTitle_En = "LABstend - Advertising Service",
            MetaTitle_Ru = "LABstend - Рекламные Услуги",

            MetaDescription_Az = "Vinil, banner, forex və digər reklam xidmətləri təklif edən peşəkar şirkət.",
            MetaDescription_En = "Professional company offering vinyl, banner, forex and other advertising services.",
            MetaDescription_Ru = "Профессиональная компания, предлагающая винил, баннер, форекс и другие рекламные услуги.",

            Keywords = "Vinil,Banner,Reklam,Forex",

            TitleAZ = "Haqqımızda",
            TitleEN = "AboutUs",
            TitleRU = "О нас",

            DescriptionAZ = "LABstend Şirkəti",
            DescriptionEN = "LABstend Company",
            DescriptionRU = "Компания Лабстенд ",

            CreatedAt = DateTime.UtcNow,
            CreatedUser = null,
            IsDeleted = false
        });

        builder.Property(x => x.MetaTitle_Az).HasMaxLength(70);
        builder.Property(x => x.MetaTitle_En).HasMaxLength(70);
        builder.Property(x => x.MetaTitle_Ru).HasMaxLength(70);

        // Meta Description
        builder.Property(x => x.MetaDescription_Az).HasMaxLength(160);
        builder.Property(x => x.MetaDescription_En).HasMaxLength(160);
        builder.Property(x => x.MetaDescription_Ru).HasMaxLength(160);

        // Keywords
        builder.Property(x => x.Keywords).HasMaxLength(500);


        builder.Property(x => x.TitleAZ)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.TitleEN)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.TitleRU)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.DescriptionAZ)
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.DescriptionEN)
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.DescriptionRU)
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        builder.HasOne(x => x.Image)
            .WithOne(i => i.AboutUs)
            .HasForeignKey<Image>(i => i.AboutUsId) 
            .OnDelete(DeleteBehavior.Cascade);

        
    }
}
