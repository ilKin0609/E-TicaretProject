using E_Ticaret_Project.Domain.Entities;
using E_Ticaret_Project.Persistence.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Ticaret_Project.Persistence.Contexts;

public class E_TicaretProjectDbContext:IdentityDbContext<AppUser>
{
    public E_TicaretProjectDbContext(DbContextOptions<E_TicaretProjectDbContext> options) : base(options)
    {

    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<AboutUs> AboutUs { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<ContactInfo> ContactInfo { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<ProductTag> ProductTags { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<SpecialRequest> SpecialRequests { get; set; }
    public DbSet<SiteSetting> SiteSetting { get; set; }
    public DbSet<KeywordSearchStat> KeywordSearchStats { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CategoryConfiguration).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
