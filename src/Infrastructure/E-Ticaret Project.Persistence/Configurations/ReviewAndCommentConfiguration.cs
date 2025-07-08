using E_Ticaret_Project.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace E_Ticaret_Project.Persistence.Configurations;

public class ReviewAndCommentConfiguration: IEntityTypeConfiguration<ReviewAndComment>
{
    public void Configure(EntityTypeBuilder<ReviewAndComment> builder)
    {
        builder.HasKey(Rc => Rc.Id);


        builder.Property(Rc => Rc.Comment)
            .IsRequired()
            .HasMaxLength(1000);

        builder.HasOne(Rc => Rc.Parent)
            .WithMany(Rc => Rc.Replies)
            .HasForeignKey(Rc => Rc.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
