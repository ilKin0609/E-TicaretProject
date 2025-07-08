using E_Ticaret_Project.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Ticaret_Project.Persistence.Configurations;

public class CategoryConfiguration:IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(Ct => Ct.Name)
            .IsRequired()
            .HasMaxLength(350);

        builder.HasOne(Ct => Ct.ParentCategory)
            .WithMany(Ct=>Ct.SubCategories)
            .HasForeignKey(Ct=>Ct.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(Ct=>Ct.Products)
            .WithOne(P=>P.Category)
            .HasForeignKey(P=>P.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
