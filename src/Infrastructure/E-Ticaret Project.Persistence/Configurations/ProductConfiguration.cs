using E_Ticaret_Project.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace E_Ticaret_Project.Persistence.Configurations;

public class ProductConfiguration: IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(P => P.Tittle)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(P => P.Description)
            .HasMaxLength(1000);

        builder.Property(P => P.Price)
            .IsRequired();


        builder.HasMany(P => P.Comments)
            .WithOne(Rc => Rc.Product)
            .HasForeignKey(Rc => Rc.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
