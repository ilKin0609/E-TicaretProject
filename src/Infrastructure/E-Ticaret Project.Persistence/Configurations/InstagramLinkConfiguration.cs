using E_Ticaret_Project.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Ticaret_Project.Persistence.Configurations;

public class InstagramLinkConfiguration : IEntityTypeConfiguration<InstagramLink>
{
    public void Configure(EntityTypeBuilder<InstagramLink> builder)
    {

        builder.Property(x => x.Link)
               .IsRequired()
               .HasMaxLength(2048);


        builder.HasIndex(x => x.Link).IsUnique();
    }
}
