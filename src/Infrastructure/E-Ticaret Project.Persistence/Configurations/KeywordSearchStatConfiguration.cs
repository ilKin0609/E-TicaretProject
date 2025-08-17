using E_Ticaret_Project.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Ticaret_Project.Persistence.Configurations;

public class KeywordSearchStatConfiguration : IEntityTypeConfiguration<KeywordSearchStat>
{
    public void Configure(EntityTypeBuilder<KeywordSearchStat> b)
    {
        b.Property(x => x.Keyword).IsRequired().HasMaxLength(150);
        b.Property(x => x.Slug).HasMaxLength(160);

        b.HasIndex(x => x.Slug)
         .IsUnique()
         .HasFilter("[IsDeleted] = 0");  // soft delete üçün

        b.HasQueryFilter(x => !x.IsDeleted);
    }
}
