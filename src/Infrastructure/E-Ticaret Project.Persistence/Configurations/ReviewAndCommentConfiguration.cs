using E_Ticaret_Project.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace E_Ticaret_Project.Persistence.Configurations;

public class ReviewAndCommentConfiguration: IEntityTypeConfiguration<ReviewAndComment>
{
    public void Configure(EntityTypeBuilder<ReviewAndComment> builder)
    {
        builder.Property(Rc => Rc.Comment)
            .IsRequired()
            .HasMaxLength(1000);
    }
}
