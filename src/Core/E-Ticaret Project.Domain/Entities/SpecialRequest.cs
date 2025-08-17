using Microsoft.AspNetCore.Http;

namespace E_Ticaret_Project.Domain.Entities;

public class SpecialRequest:BaseEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public Image? File{ get; set; }
    public string OrderAbout { get; set; }
}
