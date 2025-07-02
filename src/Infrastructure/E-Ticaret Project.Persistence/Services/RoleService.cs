using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.RoleDtos;
using E_Ticaret_Project.Application.Shared.Responses;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Security.Claims;

namespace E_Ticaret_Project.Persistence.Services;

public class RoleService:IRoleService
{
    public RoleManager<IdentityRole> _roleManager { get; }

    public RoleService(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<BaseResponse<RoleGetDto>> RoleGetByIdAsync(string RoleId)
    {
        var role = await _roleManager.FindByIdAsync(RoleId);
        if (role is null)
            return new("Role doesn't exist", HttpStatusCode.NotFound);

        var claims = await _roleManager.GetClaimsAsync(role);
        var GetPermission = new List<string>();
        foreach (var claim in claims)
        {
            GetPermission.Add(claim.Value);
        }

        var existedRole = new RoleGetDto
        {

            RoleId = role.Id,
            Name = role.Name,
            Permissions = GetPermission
        };

        return new("Role and its permissions", existedRole, HttpStatusCode.OK);


    }
    public async Task<BaseResponse<string?>> CreateRole(RoleCreateDto dto)
    {
        // Rolu yoxla, varsa error ver
        var existingRole = await _roleManager.FindByNameAsync(dto.Name);
        if (existingRole is not null)
            return new("Bu adda rol artıq mövcuddur", HttpStatusCode.BadRequest);

        // Yeni rol yarat
        var identityRole = new IdentityRole(dto.Name);
        var result = await _roleManager.CreateAsync(identityRole);

        if (!result.Succeeded)
        {
            var errorMessages = string.Join(";", result.Errors.Select(e => e.Description));
            return new(errorMessages, HttpStatusCode.BadRequest);
        }

        foreach (var permission in dto.PermissionList.Distinct())
        {
            var claimResult = await _roleManager.AddClaimAsync(identityRole, new Claim("Permission", permission));
            if (!claimResult.Succeeded)
            {
                var error = string.Join(";", claimResult.Errors.Select(e => e.Description));
                return new($"Role created,but adding permission '{permission}' failed:{error}", HttpStatusCode.PartialContent);
            }
        }
        return new("Role created succesfully", true, HttpStatusCode.Created);
    }

    public async Task<BaseResponse<string?>> UpdateRole(RoleUpdateDto dto)
    {
        var role = await _roleManager.FindByIdAsync(dto.RoleId);

        if (role is null)
        {
            return new("Role is not found", HttpStatusCode.NotFound);
        }

        if (!string.IsNullOrWhiteSpace(dto.Name) && dto.Name != role.Name)
        {
            role.Name = dto.Name;
            var nameResult = await _roleManager.UpdateAsync(role);

            if (!nameResult.Succeeded)
                return new("Name cannot update", HttpStatusCode.BadRequest);
        }

        // Permission dəyişmək istəyirsə
        if (dto.PermissionList is not null)
        {
            var currentClaims = await _roleManager.GetClaimsAsync(role);
            foreach (var c in currentClaims.Where(c => c.Type == "Permission"))
                await _roleManager.RemoveClaimAsync(role, c);

            foreach (var p in dto.PermissionList.Distinct())
                await _roleManager.AddClaimAsync(role, new Claim("Permission", p));
        }
        return new("Role succesfully updated", true, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<string?>> DeleteRole(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role is null)
            return new("Role is not found", HttpStatusCode.NotFound);

        await _roleManager.DeleteAsync(role);
        return new("Role succesfully deleted", true, HttpStatusCode.OK);
    }

}
