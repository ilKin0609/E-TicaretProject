
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using E_Ticaret_Project.Domain.Entities;

namespace E_Ticaret_Project.Persistence.Configurations;

public class SpecialRequestConfiguration: IEntityTypeConfiguration<SpecialRequest>
{
    public void Configure(EntityTypeBuilder<SpecialRequest> builder)
    {
        builder.Property(sr => sr.Name)
           .IsRequired()
           .HasMaxLength(100);

        builder.Property(sr => sr.Surname)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(sr => sr.Phone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(sr => sr.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(sr => sr.OrderAbout)
            .IsRequired()
            .HasColumnType("nvarchar(max)");
    }
}
