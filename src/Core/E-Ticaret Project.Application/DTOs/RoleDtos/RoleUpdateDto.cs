namespace E_Ticaret_Project.Application.DTOs.RoleDtos;

public record RoleUpdateDto
{
    public string RoleId { get; set; }
    public string? Name { get; set; }
    public List<string>? PermissionList { get; set; }
}
