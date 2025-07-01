using E_Ticaret_Project.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Ticaret_Project.Persistence.Contexts;

public class E_TicaretProjectDbContext:IdentityDbContext<AppUser>
{
    public E_TicaretProjectDbContext(DbContextOptions<E_TicaretProjectDbContext> options) : base(options)
    {

    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ReviewAndComment> ReviewAndComments { get; set; }
}
