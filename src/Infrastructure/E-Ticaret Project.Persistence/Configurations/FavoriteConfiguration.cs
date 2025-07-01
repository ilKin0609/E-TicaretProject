using E_Ticaret_Project.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Ticaret_Project.Persistence.Configurations;

public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> builder)
    {
        builder.HasMany(F=>F.Products)
            .WithOne(P=>P.Favorite)
            .HasForeignKey(P=>P.FavoriteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
