using E_Ticaret_Project.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Ticaret_Project.Persistence.Configurations;

public class AppUserConfiguration:IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(U => U.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(U => U.Surname)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(U => U.Company)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(U => U.Duty)
            .IsRequired()
            .HasMaxLength(100);

    }
}
