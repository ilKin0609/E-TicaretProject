namespace E_Ticaret_Project.Application.DTOs.AdminDtos;

public record UserAddRoleDto(

    string UserId,
    List<string> RolesId 
);

