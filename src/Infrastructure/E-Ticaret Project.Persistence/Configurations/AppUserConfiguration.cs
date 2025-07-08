using E_Ticaret_Project.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Ticaret_Project.Persistence.Configurations;

public class AppUserConfiguration:IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(U => U.FullName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(U => U.ProfileImageUrl)
            .HasMaxLength(300);


        builder.HasMany(U=>U.Buyers)
            .WithOne(O=>O.Buyer)
            .HasForeignKey(O=>O.BuyerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(U => U.Sellers)
            .WithOne(P => P.Owner)
            .HasForeignKey(P => P.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(U => U.Comments)
            .WithOne(Rc => Rc.User)
            .HasForeignKey(Rc => Rc.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(U => U.Favorites)
            .WithOne(F => F.User)
            .HasForeignKey(F => F.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
