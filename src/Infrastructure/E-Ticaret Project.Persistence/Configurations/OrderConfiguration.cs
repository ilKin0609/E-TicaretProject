using E_Ticaret_Project.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Ticaret_Project.Persistence.Configurations;

public class OrderConfiguration: IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(O=>O.OrderStatus)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(O => O.OrderDate)
            .IsRequired();

        builder.Property(O => O.TrackingCode)
            .IsRequired();

        builder.HasIndex(O => O.TrackingCode)
            .IsUnique();

        builder.Property(O => O.PaymentMethod)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(O => O.ShippingAddress)
            .HasMaxLength(300);

        builder.Property(O => O.ShoppingAddress)
            .IsRequired()
            .HasMaxLength(300);

        builder.HasMany(O => O.Items)
            .WithOne(Oi => Oi.Order)
            .HasForeignKey(Oi => Oi.OrderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
