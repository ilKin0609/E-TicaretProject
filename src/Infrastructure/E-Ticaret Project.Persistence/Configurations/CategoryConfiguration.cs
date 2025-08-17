using E_Ticaret_Project.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Ticaret_Project.Persistence.Configurations;

public class CategoryConfiguration:IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> b)
    {
        b.Property(x => x.NameAz).IsRequired().HasMaxLength(200);
        b.Property(x => x.NameRu).IsRequired().HasMaxLength(200);
        b.Property(x => x.NameEn).IsRequired().HasMaxLength(200);

        b.Property(x => x.Slug).IsRequired().HasMaxLength(150);

        b.Property(x => x.MetaTitleAz).HasMaxLength(255);
        b.Property(x => x.MetaTitleRu).HasMaxLength(255);
        b.Property(x => x.MetaTitleEn).HasMaxLength(255);

        b.Property(x => x.MetaDescriptionAz).HasMaxLength(1000);
        b.Property(x => x.MetaDescriptionRu).HasMaxLength(1000);
        b.Property(x => x.MetaDescriptionEn).HasMaxLength(1000);

        b.Property(x => x.Keywords).HasMaxLength(500);

        b.HasIndex(x => x.Slug).IsUnique().HasFilter("[IsDeleted] = 0"); ;

        b.HasIndex(x => new { x.ParentCategoryId, x.IsVisible, x.Order });

        b.HasIndex(x => new { x.IsVisible, x.Order });

        b.HasQueryFilter(x => !x.IsDeleted);


        b.HasOne(Ct => Ct.ParentCategory)
            .WithMany(Ct=>Ct.SubCategories)
            .HasForeignKey(Ct=>Ct.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasMany(Ct=>Ct.Products)
            .WithOne(P=>P.Category)
            .HasForeignKey(P=>P.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
