using E_Ticaret_Project.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;


namespace E_Ticaret_Project.Persistence.Configurations;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasIndex(I => I.ImageUrl)
            .IsUnique();

        builder.Property(i => i.ImageUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(i => i.IsMain)
            .IsRequired();

        builder.Property(i => i.PublicId)
            .HasMaxLength(255);

        builder.HasIndex(i => i.AboutUsId)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.HasOne(i => i.SpecialRequest)
            .WithOne(sp => sp.File)
            .HasForeignKey<Image>(i => i.SpecialRequestId)
            .OnDelete(DeleteBehavior.Cascade);


        builder.HasIndex(x => x.SpecialRequestId)
            .IsUnique(false);

        builder.HasIndex(x => x.AboutUsId)
            .IsUnique(false);
    }
}
