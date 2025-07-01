using E_Ticaret_Project.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace E_Ticaret_Project.Persistence.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(Oi => new { Oi.OrderId, Oi.ProductId });

        builder.Property(Oi => Oi.OrderCount)
            .IsRequired();
        builder.Property(Oi => Oi.FirstPrice)
            .IsRequired();

        builder.HasOne(Oi => Oi.Product)
            .WithMany(P => P.Items)
            .HasForeignKey(Oi => Oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
