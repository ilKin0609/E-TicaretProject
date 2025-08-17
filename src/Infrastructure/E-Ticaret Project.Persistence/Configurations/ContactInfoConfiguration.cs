using E_Ticaret_Project.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Ticaret_Project.Persistence.Configurations;

public class ContactInfoConfiguration:IEntityTypeConfiguration<ContactInfo>
{
    public void Configure(EntityTypeBuilder<ContactInfo> builder)
    {
        builder.HasData(new ContactInfo
        {
            Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),

            MetaTitle_Az = "Əlaqə",
            MetaTitle_En = "Contact",
            MetaTitle_Ru = "Контакт",

            MetaDescription_Az = "Bizimlə əlaqə saxlayın",
            MetaDescription_En = "Contact us",
            MetaDescription_Ru = "Свяжитесь с нами",

            Keywords = "Əlaqə, Bizimlə əlaqə, Contact, Communication",

            Title_Az = "Əlaqə məlumatları",
            Title_En = "Contact Information",
            Title_Ru = "Контактная информация",

            AddressAZ = "Bakı, Azərbaycan",
            AddressEN = "Baku, Azerbaijan",
            AddressRU= "Баку, Азербайджан",

            Phone = "+994502223344",
            Email = "info@example.com",

            MapIframeSrc = "https://maps.google.com/?q=labstend"
        });

        builder.Property(x => x.Phone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.AddressAZ)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.AddressEN)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.AddressRU)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.MapIframeSrc)
            .IsRequired()
            .HasColumnType("nvarchar(max)");
    }
}
