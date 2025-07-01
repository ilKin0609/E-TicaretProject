using E_Ticaret_Project.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Ticaret_Project.Persistence.Configurations;

public class OrderConfiguration: IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(O=>O.OrderStatus)
            .IsRequired();
        builder.Property(O => O.OrderDate)
            .IsRequired();

        builder.HasMany(O => O.Items)
            .WithOne(Oi => Oi.Order)
            .HasForeignKey(Oi => Oi.OrderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
