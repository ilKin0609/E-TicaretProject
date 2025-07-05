namespace E_Ticaret_Project.Application.DTOs.UserDtos;

public record UserAddRoleDto(

    string UserId,
    List<string> RolesId 
);

