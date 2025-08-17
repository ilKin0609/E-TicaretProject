using Microsoft.AspNetCore.Identity;

namespace E_Ticaret_Project.Domain.Entities;

public class AppUser : IdentityUser
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Company { get; set; }
    public string Duty { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshExpireDate { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public DateOnly? PartnerRequestAt { get; set; }
    public string? PasswordVault { get; set; }
    public bool isToggle { get; set; } = false;

}
