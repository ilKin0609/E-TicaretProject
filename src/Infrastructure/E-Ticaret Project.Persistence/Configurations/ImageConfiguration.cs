using E_Ticaret_Project.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace E_Ticaret_Project.Persistence.Configurations;

public class ImageConfiguration:IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasIndex(I => I.Image_Url)
            .IsUnique();

        builder.Property(I => I.Image_Url)
            .IsRequired();

        builder.HasOne(I=>I.Product)
            .WithMany(P=>P.Images)
            .HasForeignKey(I=>I.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
