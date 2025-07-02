using Microsoft.AspNetCore.Identity;

namespace E_Ticaret_Project.Domain.Entities;

public class AppUser:IdentityUser
{
    public string FullName { get; set; }
    public string? ProfileImageUrl { get; set; }
    public ICollection<Order> Buyers { get; set; }
    public ICollection<Product> Sellers { get; set; }
    public ICollection<ReviewAndComment> Comments { get; set; }
    public ICollection<Favorite> Favorites { get; set; }
}
