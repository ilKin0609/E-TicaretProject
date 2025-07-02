using E_Ticaret_Project.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Ticaret_Project.Persistence.Configurations;

public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> builder)
    {
        builder.HasKey(F => new { F.UserId, F.ProductId });

        builder.HasOne(F => F.Product)
            .WithMany(F => F.Favorites)
            .HasForeignKey(F => F.ProductId);
    }
}
