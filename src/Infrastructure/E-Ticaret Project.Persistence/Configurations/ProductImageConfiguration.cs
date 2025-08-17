using E_Ticaret_Project.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Ticaret_Project.Persistence.Configurations;

public class ProductImageConfiguration:IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> b)
    {
        b.Property(x => x.Url).HasMaxLength(1024).IsRequired();

        b.Property(x => x.PublicId).HasMaxLength(256).IsRequired();

        b.HasIndex(x => new { x.ProductId, x.IsMain })
            .IsUnique()
            .HasFilter("[IsMain] = 1");

        b.Property(x => x.SortOrder).HasDefaultValue(0);
    }
}
