namespace E_Ticaret_Project.Application.DTOs.RoleDtos;

public record RoleGetDto
{
    public string RoleId { get; set; }
    public string? Name { get; set; }

    public List<string> Permissions { get; set; } = new();
}
