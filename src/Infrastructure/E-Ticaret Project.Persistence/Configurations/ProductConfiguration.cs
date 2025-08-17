using E_Ticaret_Project.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace E_Ticaret_Project.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> b)
    {

        b.Property(x => x.SID).HasMaxLength(16).IsRequired();
        b.HasIndex(x => x.SID).IsUnique();

        b.Property(x => x.SKU).HasMaxLength(64).IsRequired();
        b.HasIndex(x => x.SKU).IsUnique();

        b.Property(x => x.PriceAZN).HasColumnType("decimal(18,2)");
        b.Property(x => x.PartnerPriceAZN).HasColumnType("decimal(18,2)");

        b.Property(x => x.TitleAz).HasMaxLength(200).IsRequired();
        b.Property(x => x.TitleEn).HasMaxLength(200);
        b.Property(x => x.TitleRu).HasMaxLength(200);

        b.Property(x => x.MetaTitleAz).HasMaxLength(60);
        b.Property(x => x.MetaTitleEn).HasMaxLength(60);
        b.Property(x => x.MetaTitleRu).HasMaxLength(60);

        b.Property(x => x.MetaDescriptionAz).HasMaxLength(160);
        b.Property(x => x.MetaDescriptionEn).HasMaxLength(160);
        b.Property(x => x.MetaDescriptionRu).HasMaxLength(160);

       
        b.HasIndex(x => x.SlugAz).IsUnique().HasFilter("[SlugAz] IS NOT NULL");

        b.HasMany(p => p.Images)
         .WithOne(i => i.Product)
         .HasForeignKey(i => i.ProductId)
         .OnDelete(DeleteBehavior.Cascade);

        b.HasMany(p => p.ProductTags)
            .WithOne(pt => pt.Product)
            .HasForeignKey(pt => pt.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
